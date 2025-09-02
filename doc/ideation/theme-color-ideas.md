# Theme Color Ideas

---

### 🎨 **Core Color Roles (Theme Anchors)**

These typically translate to `.bg-{name}`, `.text-{name}`, `.border-{name}`, etc.

- `--color-primary`: Your brand's main accent
- `--color-secondary`: A supporting accent, often used with primary
- `--color-tertiary`: Optional third accent for variety
- `--color-neutral`: For UI surfaces and text, often grays or off-whites
- `--color-accent`: A flare color for highlights or special UI moments

---

### ✅ **State-Based Colors (System Feedback)**

Inspired by Bootstrap's feedback styles and Material’s state layers.

- `--color-success`: Success messages, confirmations
- `--color-warning`: Cautionary messages or attention cues
- `--color-error`: Errors, failures, destructive actions
- `--color-info`: Informational alerts, tips, secondary feedback
- `--color-critical`: Higher-risk or dangerous actions

---

### 🌗 **Interactive States**

Useful for hover, focus, active, disabled states in components.

- `--color-hover-primary`: Primary hover state
- `--color-focus-primary`: Outline or shadow color for focus ring
- `--color-disabled`: Button or input inactive state
- `--color-selected`: For selection highlights (list items, tabs, etc.)

---

### 🧱 **Structural Colors (Surface Layers)**

Great for organizing elevation and layout depth.

- `--color-surface`: Main background
- `--color-surface-alt`: Secondary surface (modals, cards)
- `--color-surface-inverse`: Dark mode counterpart
- `--color-border`: Default border color
- `--color-divider`: For subtle separators or lines

---

### ✨ Optional/Advanced Tokens

These can future-proof your system or allow more expressive UIs.

- `--color-shadow`: For box shadows that vary per elevation
- `--color-overlay`: Dimmed layers, e.g., backdrops
- `--color-highlight`: Text selection or emphasized content
- `--color-muted`: De-emphasized text or UI

---

### 🧱 Surface-Like Semantic Tokens

| Concept        | Purpose                                                                 | Example Usage                          |
|----------------|-------------------------------------------------------------------------|----------------------------------------|
| `background`   | Base canvas behind all content                                          | `bg-background`, `text-on-background`  |
| `container`    | A grouping layer for content, often nested inside surface               | `bg-container`, `border-container`     |
| `elevation`    | Indicates depth or z-index layering                                     | `bg-elevation-1`, `shadow-elevation-2` |
| `overlay`      | Semi-transparent layer over surfaces (e.g. modals, tooltips)            | `bg-overlay`, `bg-overlay-dark`        |
| `card`         | Specific surface used for card components                               | `bg-card`, `hover:bg-card-hover`       |
| `panel`        | Used for sidebars, drawers, or floating UI elements                     | `bg-panel`, `border-panel`             |
| `canvas`       | The outermost background layer, often behind everything else            | `bg-canvas`, `text-on-canvas`          |

---
