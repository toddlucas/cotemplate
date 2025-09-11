# DB security

Absolutely—logical isolation in a single Postgres works well with Tableau as long as you design **row-level security (RLS)** and **connection/session scoping** cleanly. Below are pragmatic patterns you can mix and match. I'll show concrete SQL and how to wire Tableau.

# 0) Terms

* Every row has: `group_id` (reseller/partner) and `tenant_id` (customer organization).
* Tableau connects as a low-privilege DB user (never superuser).
* We'll scope each Tableau session to a group/tenant via either **session variables** or **per-tenant roles**.

# 1) Core: Row-Level Security (RLS)

Enable RLS and index your scope keys:

```sql
-- Example table
CREATE TABLE txn (
  id bigserial PRIMARY KEY,
  group_id uuid NOT NULL,
  tenant_id uuid NOT NULL,
  account_id uuid NOT NULL,
  amount numeric(18,2) NOT NULL,
  occurred_at timestamptz NOT NULL,
  -- ...
  CHECK (group_id IS NOT NULL AND tenant_id IS NOT NULL)
);

CREATE INDEX ON txn (group_id, tenant_id);
ALTER TABLE txn ENABLE ROW LEVEL SECURITY;
```

Use a **session variable** pattern (simple and Tableau-friendly):

```sql
-- One-time: allow custom settings namespace
ALTER DATABASE yourdb SET custom_variable_classes = 'app'; -- (old style, optional)
-- Modern: you can use any key, e.g., 'app.group_id', no predeclare needed

-- Policies using current_setting()
CREATE POLICY rls_txn_read
ON txn FOR SELECT
USING (
  current_setting('app.group_id', true) IS NOT NULL
  AND group_id = uuid(current_setting('app.group_id'))
  AND (
    current_setting('app.tenant_ids', true) IS NULL
    OR tenant_id = ANY (string_to_array(current_setting('app.tenant_ids'), ',')::uuid[])
  )
);
```

> This lets you scope by **group** (reseller) and optionally by a **list of tenants** (customers).

# 2) Tableau wiring (Initial SQL)

In Tableau, set **Initial SQL** on the connection so each session is scoped:

```sql
-- Initial SQL (examples)
SET app.group_id = '2b4f8e5a-...-...';
-- Single tenant
SET app.tenant_ids   = '3f0c1b63-...-...';
-- Or multiple tenants (comma-separated)
-- SET app.tenant_ids   = 'tenant-uuid-1,tenant-uuid-2,tenant-uuid-3';
```

Tableau will then only "see" rows allowed by the RLS policy.
For **group-level rollups** across many tenants, set `app.tenant_ids` to a CSV of permitted tenants (or leave it NULL to allow all tenants in that group, if your policy allows that).

# 3) Alternative scoping: Per-group/tenant DB roles (no session vars)

If you prefer not to rely on Initial SQL, use **role-mapped RLS**:

Mapping table:

```sql
CREATE TABLE db_role_scope (
  db_role name PRIMARY KEY,
  group_id uuid NOT NULL,
  tenant_ids uuid[] NULL  -- NULL means all tenants in group
);
```

Policy uses `current_user`:

```sql
CREATE POLICY rls_txn_read_by_role
ON txn FOR SELECT
USING (
  EXISTS (
    SELECT 1
    FROM db_role_scope s
    WHERE s.db_role = current_user
      AND s.group_id = txn.group_id
      AND (
        s.tenant_ids IS NULL
        OR txn.tenant_id = ANY (s.tenant_ids)
      )
  )
);
```

Then you create a **Tableau login per group** (or per advisor) and populate `db_role_scope` accordingly. No Initial SQL needed.

# 4) View-based exposure (nice with Tableau data sources)

Even with RLS, expose **reporting views** so your BI model is stable and you can mask columns:

```sql
CREATE VIEW rpt_txn_secure WITH (security_barrier=true) AS
SELECT id, occurred_at, amount, tenant_id, group_id  -- omit sensitive cols
FROM txn;

GRANT SELECT ON rpt_txn_secure TO tableau_reader;
```

> Because RLS is on the base table, it still applies when selecting from the view.

For heavy queries, create **materialized views per group** (or even per tenant) refreshed by your ETL, each protected by RLS or placed in a schema with grants aligned to the group.

# 5) Column-level security & masking

Use views to redact PII:

```sql
CREATE VIEW rpt_people_secure AS
SELECT
  person_id,
  tenant_id,
  CASE
    WHEN current_setting('app.role', true) = 'advisor' THEN left(email, 3) || '***'
    ELSE '***redacted***'
  END AS email_masked
FROM people;
```

Or separate sensitive columns into a sibling table with tighter grants.

# 6) Performance tips (important for BI)

* **Composite indexes:** `(group_id, tenant_id, occurred_at)` and any common filter keys.
* **Partitioning:** Partition large fact tables by `group_id` or by time, then keep RLS; this prunes data early.
* **Materialized rollups:** Daily/Monthly summaries (per tenant, per group) for dashboard speed.
* **Analyze & VACUUM** routinely; ensure plans use your scope indexes.

# 7) Operational considerations with Tableau

* **Connection pools:** Tableau Server may reuse sessions; **Initial SQL** runs per connection creation, not per query. If your scoping changes per workbook/user, use **separate data sources** or **separate credentials** per group/tenant, or embed dynamic Initial SQL via parameters.
* **Extracts vs live:** For extracts, set the scope on the **extract connection**; create one extract per group (or per advisor scope) to avoid data leakage.
* **Testing:** Create test users for (a) group admin, (b) tenant-limited advisor, (c) read-only share; verify row counts differ as expected.
* **No superusers:** Ensure the Tableau DB user is **not** `bypassrls` and has no `SUPERUSER`/`REPLICATION`/`BYPASSRLS`. Grant only `CONNECT` + `USAGE` on schema + `SELECT` on reporting views.

# 8) Security, auditing, compliance

* Turn on **pgaudit** (or log\_statement = 'all' for sensitive environments) to record `SET app.*` and `SELECT` on reporting views.
* Include `{group_id, tenant_id, actor_id}` in an **access log** table populated via a lightweight ` SECURITY DEFINER` function wrapper if you want in-DB auditing for BI reads.
* Encrypt sensitive fields (pgcrypto) if required; RLS still applies.

# 9) Patterns to choose from (quick matrix)

| Pattern                              | Tableau Setup                                   | Pros                                     | Cons                                                       | When to use                                  |
| ------------------------------------ | ----------------------------------------------- | ---------------------------------------- | ---------------------------------------------------------- | -------------------------------------------- |
| **RLS + Initial SQL (session vars)** | One DSN; set `SET app.group_id`, `app.tenant_ids` | Simple, flexible per session             | Must manage Initial SQL per source; beware pooled sessions | Small/medium deployments; many tenant scopes    |
| **RLS + per-group DB users**        | Separate credentials per group                 | No Initial SQL; clear isolation by login | More users to manage                                       | MSP/white-label with clear group boundaries |
| **Views + RLS**                      | Point Tableau to curated views                  | Stable schema, column masking            | Still need scoping via RLS (session or role)               | Most deployments (best practice)             |
| **Per-group materialized views**    | One view per group                             | Fast dashboards                          | More ETL/refresh logic                                     | Large datasets, strict SLAs                  |

---

## Minimal working example (combine #1 + #4)

1. Enable RLS and create policy using session vars (above).
2. Create curated reporting views (`rpt_*`).
3. Create a **single** Tableau login `tableau_reader`, grant `SELECT` on `rpt_*`.
4. In each Tableau data source, set Initial SQL:

```sql
SET app.group_id = 'GROUP-UUID-HERE';
SET app.tenant_ids   = 'TENANT-UUID-1,TENANT-UUID-2';
```

5. Publish separate data sources per group/tenant-scope.

This gives you secure, performant multi-tenant reporting from one Postgres—with clean governance and minimal friction for the BI team.

If you want, I can draft the **exact SQL/bootstrap script** (roles, grants, policies, sample views) you can run in dev to prove this out quickly.
