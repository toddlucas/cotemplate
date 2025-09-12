#!/usr/bin/env python3
import re
from pathlib import Path
import argparse
import sys
import shutil

UP_BEGIN  = "// >>> BEGIN MIGRATION UP (AUTO)"
UP_END    = "// <<< END MIGRATION UP (AUTO)"
DOWN_BEGIN= "// >>> BEGIN MIGRATION DOWN (AUTO)"
DOWN_END  = "// <<< END MIGRATION DOWN (AUTO)"

def load_text(p: Path) -> str:
    try:
        return p.read_text(encoding="utf-8")
    except UnicodeDecodeError:
        # fallback if the file contains BOM or other encoding
        return p.read_text(encoding="utf-8-sig")

def save_text(p: Path, s: str):
    p.write_text(s, encoding="utf-8")

def with_markers(content: str, begin: str, end: str) -> str:
    # ensure there’s a trailing newline inside markers for neat diffs
    content = content.rstrip() + "\n"
    return f"{begin}\n{content}{end}"

def indent_block(block: str, indent: str) -> str:
    # indent each non-empty line with the provided indent
    lines = block.splitlines()
    return "\n".join((indent + ln if ln.strip() else "") for ln in lines)

def replace_method_body(src: str, method_name: str, body_with_markers: str) -> str:
    """
    Replace body of:
      protected override void <method_name>(MigrationBuilder migrationBuilder)
    with body_with_markers. Preserves brace style and indentation.
    """
    # Regex captures:
    # 1) leading whitespace + signature + opening brace
    # 2) the existing body (non-greedy)
    # 3) the closing brace
    # Notes:
    #  - [\s\S] to allow multiline
    #  - We allow any whitespace between tokens to be robust
    pat = re.compile(
        rf"""
        (^[ \t]*protected[ \t]+override[ \t]+void[ \t]+{method_name}
           [ \t]*\([^\)]*\)[ \t]*\{{[ \t]*\n)   # group 1: signature + opening brace + newline
        ([\s\S]*?)                               # group 2: current body (non-greedy)
        (^([ \t]*)\}})                           # group 3: closing brace line; group 4: its indent
        """,
        re.MULTILINE | re.VERBOSE,
    )

    def _repl(m: re.Match) -> str:
        sig_and_open = m.group(1)
        closing_brace_line = m.group(3)
        closing_indent = m.group(4)

        # Choose a reasonable inner indent: if closing brace has N spaces, body gets same indent
        inner_indent = closing_indent
        # If the opening line shows a different indent, either works; using closing brace indent is common.

        # Remove any prior marked region to keep updates clean
        existing_body = m.group(2)
        existing_body_clean = re.sub(
            rf"^[ \t]*{re.escape(UP_BEGIN)}[\s\S]*?{re.escape(UP_END)}[ \t]*\n?|"
            rf"^[ \t]*{re.escape(DOWN_BEGIN)}[\s\S]*?{re.escape(DOWN_END)}[ \t]*\n?",
            "",
            existing_body,
            flags=re.MULTILINE,
        )

        # Prepare the new body (indented)
        body_indented = indent_block(body_with_markers, inner_indent)
        # Ensure there’s a trailing newline before the closing brace
        if not body_indented.endswith("\n"):
            body_indented += "\n"

        # If there was other content (outside our markers), keep it above our block
        preserved = existing_body_clean.rstrip()
        if preserved:
            preserved = indent_block(preserved + "\n\n", inner_indent)
            new_body = preserved + body_indented
        else:
            new_body = body_indented

        return sig_and_open + new_body + closing_brace_line

    (result, n) = pat.subn(_repl, src, count=1)
    if n == 0:
        raise RuntimeError(f"Could not find method '{method_name}' to replace.")
    return result

def main():
    ap = argparse.ArgumentParser(description="Insert/replace EF Core Migration Up/Down bodies.")
    ap.add_argument("file", help="Path to the .cs migration file")
    ap.add_argument("--up", help="Path to a file containing the Up() body (C# lines).", default=None)
    ap.add_argument("--down", help="Path to a file containing the Down() body (C# lines).", default=None)
    ap.add_argument("--up-text", help="Literal text for Up() body (ignored if --up is provided).", default=None)
    ap.add_argument("--down-text", help="Literal text for Down() body (ignored if --down is provided).", default=None)
    ap.add_argument("--no-backup", action="store_true", help="Do not write a .bak backup of the original.")
    args = ap.parse_args()

    path = Path(args.file)
    if not path.exists():
        print(f"File not found: {path}", file=sys.stderr)
        sys.exit(1)

    # Load bodies
    if args.up:
        up_body = load_text(Path(args.up))
    else:
        up_body = args.up_text or """// your Up() operations go here
migrationBuilder.Sql("SELECT 1;");"""

    if args.down:
        down_body = load_text(Path(args.down))
    else:
        down_body = args.down_text or """// your Down() operations go here
migrationBuilder.Sql("SELECT 0;");"""

    # Wrap with markers (for idempotency)
    up_block = with_markers(up_body, UP_BEGIN, UP_END)
    down_block = with_markers(down_body, DOWN_BEGIN, DOWN_END)

    original = load_text(path)

    try:
        updated = replace_method_body(original, "Up", up_block)
        updated = replace_method_body(updated, "Down", down_block)
    except RuntimeError as e:
        print(str(e), file=sys.stderr)
        sys.exit(2)

    if not args.no_backup:
        shutil.copyfile(path, f"{path}.bak")

    save_text(path, updated)
    print(f"Updated: {path}")
    if not args.no_backup:
        print(f"Backup:  {path}.bak")

if __name__ == "__main__":
    main()
