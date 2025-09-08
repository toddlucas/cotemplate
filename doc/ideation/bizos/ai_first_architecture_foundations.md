# AI‑First Architecture Foundations

A blueprint for building a modular, AI‑native platform that powers governance, reporting, cap‑table modeling, and multi‑entity workflows.

---

## 1) Product Tenets
- **Structured First, Generative Second:** Canonical data lives in a normalized graph; generative AI drafts and explains, but never becomes the source of truth.
- **Human‑in‑the‑Loop:** All AI outputs ship as drafts with clear provenance and one‑click acceptance/edit.
- **Explainable by Default:** Every insight, draft, and reminder carries citations to docs/records and a reproducible calculation trail.
- **Composable:** Common services (ingestion, memory, orchestration, tasks) are reused across all modules.

---

## 2) Core Domain Model (Knowledge Graph)
**Entities:** Company/LLC/Trust/Individual/Account/Asset/Policy/Obligation/Document.

**Relationships (typed edges):** owns(%) → Entity↔Entity/Person, controls → Person↔Entity, obligation_of → Obligation↔Entity, secures → Policy↔Asset, references → Document↔{Entity, Obligation}.

**Data rules:**
- Versioned records (effective‑from / effective‑to) for point‑in‑time queries (e.g., ownership on a given date).
- Jurisdictional metadata (state, country) on entities/obligations.
- Monetary values always with currency + as‑of date.

---

## 3) Document Ingestion & Structuring Pipeline
**Stages:**
1. **Acquire:** Upload, email‑in, cloud connectors (Drive/Dropbox), e‑sign webhooks.
2. **Pre‑process:** OCR, layout analysis, classification (SAFE, Operating Agreement, Insurance Policy, Filing Receipt).
3. **Extract:** LLM+regex+templates to pull entities, dates, parties, percentages, dollar amounts; confidence scoring.
4. **Normalize:** Map to schema (Units/Percentages/Dates/Currency) and link to graph nodes; surface low‑confidence fields for review.
5. **Enrich:** Jurisdiction calendars (state deadlines), counterparty lookups, instrument terms libraries.

**Output:** Structured deltas + attachments + provenance (doc id, page span, extraction method, confidence).

---

## 4) AI Orchestration Layer (Copilot API)
**Purpose:** Centralize prompts, tools, and guardrails; route requests to skills.

**Components:**
- **Skills Registry:** `summarize_document`, `explain_dilution`, `draft_resolution`, `detect_discrepancy`, `classify_transaction`, `forecast_cashflow`, `schedule_obligation`.
- **Context Builder:** Pulls graph slices, recent docs, and user prefs; enforces least‑privilege data access.
- **Tool Execution:** Deterministic functions with typed inputs/outputs (JSON schemas) for calculations or system actions.
- **Policy Filters:** Redact PII where required; block unsafe actions; ensure jurisdictional disclaimers.
- **Feedback Loop:** Thumbs‑up/down and inline edits feed supervised fine‑tunes and prompt tweaks.

---

## 5) Memory & Retrieval
- **Profile Memory:** User/org preferences, typical entities, recurring tasks.
- **Case Memory:** Thread‑scoped cache of active docs, decisions, and calculations.
- **Vector Store:** Embeddings for full‑text retrieval over uploaded docs; chunked with page/section anchors for citation.
- **Event Timeline:** Append‑only log of state changes (ownership, obligations, balances) for “why did this change?” explanations.

---

## 6) Tasks, Obligations & Scheduler (Action Space)
- **Unified Task Model:** `{id, subject, type, entity_id, due_at, recurrence, source, severity, assignee, status, links[]}`
- **Generators:** Rules from ingestion (e.g., annual report due), jurisdiction catalogs, and user‑defined SLAs.
- **AI Interfaces:** `schedule_obligation()`, `update_task()`, `explain_task_origin()`; all idempotent and auditable.

---

## 7) Explainability & Auditability
- **Provenance Graph:** Each AI output links to inputs (docs, fields, calculations) with hashes and timestamps.
- **Recompute:** Deterministic tools re‑run from captured inputs for reproducibility.
- **Change Review:** Side‑by‑side diffs when accepting AI‑proposed updates to the graph.

---

## 8) Permissions, Multi‑Tenancy & White Label
- **Tenant Layer (White Label):** Each branded deployment is a tenant. Controls branding, billing, domains, and default policies.
- **Organization Layer:** Within a tenant, multiple organizations (families, businesses, trusts) can be managed. Each owns its own entity graph.
- **User Identity:** Users (members, advisors, admins) are global identities. They can be granted roles across multiple orgs and tenants.
- **Advisors Across Orgs/Tenants:** Advisors can work across organizations and even across tenants (via global identity federation or scoped invites). One login, multiple contexts.
- **ACL Model:** Row/edge‑level security enforced at the graph API. Context Builder ensures AI only sees nodes/edges allowed by the current user’s role.
- **Isolation Strategies:**
  - Logical isolation (tenant/org IDs on rows/edges) for scalability.
  - Hard isolation (dedicated DB per tenant) for high‑sensitivity clients.
  - Hybrid approach: small clients use logical; enterprise wealth managers can opt into dedicated.
- **Cross‑Org Views:** Wealth managers can query roll‑ups across all orgs they manage within a tenant. Advisors can switch contexts without duplicated accounts.

---

## 9) Calculators & Deterministic Engines
- **Equity Math Engine:** SAFEs/notes, valuation caps, pro‑rata, option pools, waterfalls; fully typed and tested.
- **Consolidation Engine:** Multi‑entity P&L/BS, intercompany eliminations, FX if needed; documented rules tables.
- **Scenario Engine:** Parameterized “what‑ifs” with snapshots and comparisons.

---

## 10) Integrations Layer
- **Connectors:** QBO/Xero, banks (Plaid), cap‑table (Carta/Pulley export), e‑sign, storage.
- **Mapping:** AI‑assisted field mapping with human approval; persistent mapping profiles.
- **Sync Contracts:** Idempotent upserts; conflict detection with human arbitration.

---

## 11) Observability & Quality
- **Data Quality Monitors:** Missing fields, dangling edges, unit/currency inconsistencies.
- **AI QA:** Golden‑set evaluations for extractors and skills (precision/recall, BLEU/ROUGE where relevant).
- **Product Analytics:** Track time‑to‑value (first extraction, first task, first consolidated report) and draft acceptance rates.

---

## 12) Security & Compliance
- **Encryption:** At rest + in transit; field‑level encryption for sensitive identifiers.
- **Secrets & Keys:** KMS/SM; scoped, rotated; just‑in‑time access for connectors.
- **Compliance Posture:** SOC 2 roadmap; data residency options; model privacy (no training on customer data without consent).

---

## 13) Developer Experience
- **Public Graph API:** Read/Write with webhooks for state changes.
- **Automation SDK:** Type‑safe client for skills and tools; local sandbox with synthetic data.
- **Template Packs:** Governance templates, jurisdiction calendars, modeling presets.

---

## 14) Incremental Delivery Plan
**Milestone A: Foundation**
- Graph schema + CRUD API; basic ingestion (classification + key‑value extraction); tasks model; minimal copilot with `summarize_document` and `schedule_obligation`.

**Milestone B: First Wedges**
- Governance drafts (resolutions/minutes) with citations; QBO connector + basic consolidation; cap‑table calculator v1.

**Milestone C: Depth & Reliability**
- Provenance graph UI; deterministic recompute; mapping assistant; anomaly detection.

**Milestone D: Platform**
- Cross‑entity scenario engine; advisor collaboration; marketplace for templates/integrations.

---

## 15) Tech Stack Suggestions (illustrative)
- **Graph/Data:** Postgres + pgvector or Neo4j; Prisma/Hasura; temporal tables for versioning.
- **Ingestion:** Python workers (Pydantic, spaCy/PDF libs) + queue (Celery/SQS).
- **LLM Orchestration:** TypeScript/Node service (LangChain.js / custom) with tools as JSON‑schema functions; eval harness (Evals/Promptfoo).
- **Retrieval:** OpenSearch/Weaviate/PGVector; chunking + citations.
- **Frontend:** React/Next.js; Tailwind; tiptap/MDX editor; audit trails and diff views baked in.

---

### Guiding Insight
Build the **graph + ingestion + orchestration** substrate once. Everything else—governance drafts, consolidation, dilution explanations, wealth narratives—are just **skills** and **deterministic tools** plugged into that substrate with auditability, human review, and flexible multi‑tenancy for both direct users and white‑label partners.



---

## 16) Multi‑Tenancy & White‑Label Architecture
**Goals:** Isolate data per brand (tenant), support multiple organizations per tenant (families/clients), and allow advisors to work across orgs and even across tenants with least‑privilege access.

### Hierarchy & Scoping
- **Tenant (White‑Label Instance):** Branding, billing plan, custom domain, policy defaults, feature flags.
- **Organization (under Tenant):** Container for entities, people, obligations, documents, tasks, and reports.
- **Graph Scoping:** Every node/edge carries `{tenant_id, org_id}`; default queries are auto‑scoped.

### Identity & Roles
- **Global Identity:** Users (advisors, clients, staff) exist once globally; attach **role grants** per `{tenant_id, org_id}`.
- **Federation/SSO:** Support tenant‑managed SSO (SAML/OIDC) and advisor “linked accounts” for cross‑tenant work via one login.
- **Roles & Permissions:** Org‑scoped RBAC + edge‑level ACLs (e.g., view ownership edges but not bank account nodes). Presets: `TenantAdmin`, `OrgAdmin`, `Advisor`, `Member`, `ReadOnly‑Share`.

### Data Isolation Strategies
- **Logical Isolation (default):** Single cluster; strict row/edge‑level security by `{tenant_id, org_id}` with automated query guards.
- **Hard Isolation (premium):** Dedicated DB/cluster per tenant (optionally per region) for large wealth managers or sensitive deployments.
- **Hybrid:** Shared control plane; per‑tenant data plane for high‑value clients.

### AI Context Builder & Privacy
- Context builder **enforces scopes** before any prompt/tool call; only the permitted graph slice is loaded.
- **Provenance & Audit** include `{tenant_id, org_id, actor_id}` to trace who/what produced an output.
- **Prompt Redaction:** Strip cross‑tenant PII; optional differential privacy / anonymized benchmarking.

### Cross‑Org / Cross‑Tenant UX
- **Roll‑Ups:** Tenant‑level dashboards can aggregate across orgs (families) where permitted (e.g., firm‑wide AUM, filings pipeline).
- **Advisor Workspace:** Single inbox of tasks across authorized orgs/tenants; quick‑switch context without data leakage.
- **Share Packs:** Time‑boxed, read‑only views for outside CPAs/attorneys at the org level.

### Configuration & Branding
- Tenant‑level theme/brand assets, email templates, document headers/footers, and domain (e.g., `client.firm.com`).
- **Policy Packs:** Jurisdiction catalogs, document templates, and naming conventions can be tenant‑overridden.

### Residency, Compliance & Billing
- **Regional Residency:** Pin a tenant’s storage/compute to a region for compliance.
- **Compliance Posture:** SOC 2 controls audited per tenant (logical) or per cluster (hard isolation).
- **Billing:** Metering rolls up at tenant; module usage by org for chargebacks.

### Migration & Portability
- **Export/Import:** JSON/CSV exports of graph slices by org; tenant‑level backups; redaction filters for portability.
- **Tenant Moves:** Support org re‑homing between tenants with re‑keyed `{tenant_id}` and preserved provenance.

> **Design Principle:** Treat tenant/org scope as a **first‑class dimension** in the graph, scheduler, and copilot. All read/write paths (including AI tools) must prove scope before execution.

