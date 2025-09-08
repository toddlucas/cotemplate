# Product Requirements Document (PRD)
**Product**: [Working Title – e.g. “Core Platform”]  
**Version**: 0.2 (Draft)  
**Date**: September 7, 2025  
**Author**: Todd Lucas  

---

## 1. Overview
**Purpose**  
Build an AI-first core platform to help underserved financial niches manage complex organizational structures, compliance, and workflows. The MVP provides a foundation of modules (organizations, entities, people/advisors, tasks/checklists, reporting) that can be tailored to specific markets via add-on modules in later phases.  

**Target Audience**  
Early adopters in underserved financial verticals such as:  
- Small wealth managers and RIAs  
- Multi-LLC/trust holding structures (family office light)  
- Small real estate fund operators  
- Professional services firms with multi-entity operations  

**Vision**  
Deliver a system where AI is the default interface: customers describe needs in natural language, and the system generates entities, workflows, and reports — reducing friction in setup and ongoing management.  

---

## 2. Goals & Non-Goals
**Goals**  
- Support creation and management of organizations, entities, and people/advisors.  
- Provide AI-assisted checklists and compliance task tracking.  
- Offer basic dashboards and reporting, with AI-enabled insights.  
- Deliver secure, multi-tenant architecture with logical isolation.  
- Enable Tableau/BI connections with row-level security.  

**Non-Goals (MVP)**  
- Full niche-specific regulatory content (to come in later modules).  
- Advanced integrations (e.g. payroll, accounting, external CRMs).  
- Deep customization of workflows (basic AI-assisted customization only).  

---

## 3. Product Scope
**Core Modules (Phase 1)**  
1. **Organizations** – logical grouping for tenants (white-label option ready).  
2. **Entities** – LLCs, trusts, corporations, etc.  
3. **People/Advisors** – individuals linked to one or more orgs/entities.  
4. **Tasks/Checklists** – AI-generated compliance and operational checklists.  
5. **Reporting/Dashboards** – org/entity roll-ups, AI-assisted summaries.  

**AI-First Layer**  
- Natural language assistant for setup (e.g., “Create an LLC under Org A with Todd as manager”).  
- AI-generated checklists (state filings, compliance reminders).  
- AI-assisted reporting (summarize across entities, flag missing tasks).  

**Integrations (Phase 1)**  
- Basic export to Excel/CSV.  
- BI connector (read-only Postgres access with row-level security).  

---

## 4. Data Design for Core Modules

### Organizations
- **id** (UUID)
- **name** (string)
- **description** (text)
- **owner_id** (FK → People)
- **settings** (JSON – branding, preferences)
- **created_at**, **updated_at** (timestamps)

### Entities
- **id** (UUID)
- **organization_id** (FK → Organizations)
- **name** (string)
- **type** (enum: LLC, Corp, Trust, Partnership, Other)
- **jurisdiction** (string – state/country)
- **formation_date** (date)
- **status** (enum: active, inactive, dissolved)
- **metadata** (JSON – flexible storage for niche attributes)
- **created_at**, **updated_at**

### People/Advisors
- **id** (UUID)
- **first_name**, **last_name** (string)
- **email** (string)
- **phone** (string)
- **role** (enum: owner, manager, advisor, staff)
- **linked_entities** (M:N via join table Entity_People)
- **linked_organizations** (M:N via join table Org_People)
- **permissions** (JSON – granular access rights)
- **created_at**, **updated_at**

### Tasks/Checklists
- **id** (UUID)
- **entity_id** (FK → Entities, nullable)
- **organization_id** (FK → Organizations)
- **title** (string)
- **description** (text)
- **category** (enum: compliance, filing, license, finance, custom)
- **due_date** (date)
- **status** (enum: open, in_progress, completed, overdue)
- **assigned_to** (FK → People)
- **source** (enum: AI, manual, template)
- **metadata** (JSON – AI prompts, references)
- **created_at**, **updated_at**

### Reporting/Dashboards
- **id** (UUID)
- **organization_id** (FK → Organizations)
- **type** (enum: compliance_summary, task_summary, entity_rollup)
- **data** (JSON – computed metrics)
- **generated_by** (enum: system, AI, user)
- **created_at**, **updated_at**

---

## 5. User Stories
**Organizations & Entities**  
- As a **wealth manager**, I want to create multiple organizations (white-label), so I can separate clients under different brands.  
- As a **founder**, I want to register multiple entities (LLCs, trusts), so I can manage my holdings in one system.  

**People/Advisors**  
- As an **advisor**, I want to be linked to multiple organizations, so I can work across clients securely.  
- As an **owner**, I want to assign roles and permissions to people, so access is controlled.  

**Tasks/Checklists**  
- As an **entity owner**, I want AI-generated compliance checklists, so I don’t miss required filings.  
- As a **manager**, I want reminders for expiring licenses, so deadlines aren’t missed.  

**Reporting/Dashboards**  
- As a **CFO**, I want a consolidated view of tasks across entities, so I can spot risks.  
- As a **wealth manager**, I want AI-generated summaries of client orgs, so I can prepare quickly for meetings.  

---

## 6. Functional Requirements
- CRUD for organizations, entities, people/advisors.  
- Role-based access control with per-org permissions.  
- Task/checklist management (AI-generated, editable).  
- Notifications/reminders (email in MVP).  
- Reporting with drill-downs (entity → org → portfolio).  
- AI assistant integrated across modules (chat-based + contextual actions).  

---

## 7. AI-First Design Principles
- **Default entry point**: chat/assistant guides setup & usage.  
- **Human-in-loop**: all AI-generated items are editable before being saved.  
- **Feedback loop**: user edits logged to improve prompt templates/models.  
- **Transparency**: AI outputs show source templates/rules when applicable.  

---

## 8. UX/UI Requirements
- Clean, modular dashboard (React + Tailwind).  
- AI assistant accessible from every page.  
- Entity/people/task lists with search & filters.  
- Reporting dashboard with AI “summary” button.  

---

## 9. Technical Requirements
- **Architecture**:  
  - Frontend: React (Next.js optional for SSR/SEO).  
  - API: .NET Core (REST/GraphQL).  
  - Database: Postgres with Row-Level Security for multi-tenancy.  
- **AI Stack**:  
  - Orchestration: LangChain.js or direct OpenAI SDK.  
  - Models: GPT-5 for general reasoning; specialized extraction prompts for document intake.  
- **Security**:  
  - Logical tenant isolation in DB.  
  - Encryption at rest and in transit.  
- **Deployments**:  
  - Docker-based microservices.  
  - Cloud (AWS/GCP).  

---

## 10. Success Metrics
- **Adoption**: 10 pilot orgs onboarded within first 90 days.  
- **Engagement**: 70%+ of tasks created via AI assistant.  
- **Retention**: 60% of orgs active at 90 days.  
- **Accuracy**: <15% of AI-generated checklist items marked as irrelevant.  

---

## 11. Risks & Mitigations
- **AI hallucinations** → Constrain outputs with templates + review.  
- **Security concerns** → Row-level security + role-based access.  
- **Overgeneralization** → Focus MVP on 1-2 niches (e.g., wealth managers + real estate).  
- **Adoption friction** → Emphasize natural language setup vs manual form filling.  

---

## 12. Open Questions
- Which niche to target first: wealth managers, family office light, or real estate funds?  
- Should AI assistant be embedded as chat only, or also inline (e.g., “AI suggest” buttons)?  
- To what extent should we allow white-label branding in MVP?


---

## 12. Data Model (MVP)
> Postgres with logical multi-tenancy (RLS). IDs are UUIDv7. Timestamps are UTC. All tables include `created_at`, `updated_at`, `created_by`, `updated_by`, and `is_deleted` (soft delete). Optional extensibility via `metadata JSONB`.

### 12.1 Tenancy & Branding
- **tenant**  
  `id`, `name`, `slug` (unique), `status` (active/suspended), `billing_plan`, `default_locale`, `timezone`, `metadata`
- **brand** (white‑label per tenant; optional)  
  `id`, `tenant_id FK`, `name`, `logo_url`, `primary_color`, `secondary_color`, `domain`, `metadata`  
  *Indexes*: (`tenant_id`), (`domain` unique where not null)

### 12.2 Organizations & People
- **org** (customer-facing container; allows sub‑orgs if needed later)  
  `id`, `tenant_id FK`, `name`, `code` (human identifier), `parent_org_id FK NULL`, `status`, `metadata`  
  *Indexes*: (`tenant_id`, `name`), (`tenant_id`, `code` unique)
- **person**  
  `id`, `tenant_id FK`, `given_name`, `family_name`, `email` (unique within tenant), `phone`, `dob` NULL, `addresses JSONB`, `auth_provider_id` NULL, `metadata`  
  *Indexes*: (`tenant_id`, `email` unique)
- **org_member** (many‑to‑many person↔org with roles)  
  `id`, `tenant_id FK`, `org_id FK`, `person_id FK`, `role` (enum: owner, admin, manager, viewer, advisor, external), `status`, `start_at`, `end_at`, `metadata`  
  *Unique*: (`org_id`, `person_id`) active window  
  *Indexes*: (`tenant_id`, `org_id`), (`tenant_id`, `person_id`)

### 12.3 Entities (Legal)
- **entity**  
  `id`, `tenant_id FK`, `org_id FK`, `name`, `legal_name`, `entity_type` (enum: llc, c_corp, s_corp, lp, llp, trust, sole_prop, plc, non_profit, spv, other), `formation_date`, `jurisdiction_country`, `jurisdiction_region` (ISO‑3166‑2), `ein` NULL, `state_file_number` NULL, `registered_agent JSONB`, `ownership_model` (enum: member_managed, manager_managed, board_managed, trustee_managed), `status` (draft, active, dissolved, merged), `metadata`  
  *Indexes*: (`tenant_id`, `org_id`), (`tenant_id`, `status`), GIN on (`metadata`)
- **entity_role** (who relates to an entity; time‑bounded)  
  `id`, `tenant_id FK`, `entity_id FK`, `person_id FK`, `role` (enum: owner, member, manager, director, officer, trustee, beneficiary, advisor, attorney, accountant, registered_agent_contact, signatory), `equity_percent` NUMERIC(6,4) NULL, `units_shares` NUMERIC NULL, `start_at`, `end_at`, `metadata`  
  *Indexes*: (`tenant_id`, `entity_id`), (`tenant_id`, `person_id`)
- **entity_relationship** (entity↔entity graph)  
  `id`, `tenant_id FK`, `parent_entity_id FK`, `child_entity_id FK`, `relationship_type` (enum: owns, controls, subsidiary_of, gp_of, lp_of, trustee_of, beneficiary_of, manager_of, advisor_to, spv_for), `percent_ownership` NULL, `start_at`, `end_at`, `metadata`  
  *Unique*: (`parent_entity_id`, `child_entity_id`, `relationship_type`, active_window)

### 12.4 Documents & Evidence (lightweight for MVP)
- **document**  
  `id`, `tenant_id FK`, `org_id FK NULL`, `entity_id FK NULL`, `person_id FK NULL`, `title`, `category` (enum: formation, compliance, tax, contract, id, other), `storage_uri`, `mime_type`, `hash`, `uploaded_by`, `uploaded_at`, `metadata`  
  *Indexes*: (`tenant_id`, `entity_id`), (`tenant_id`, `category`)
- **extracted_field** (AI extraction results; revisionable)  
  `id`, `tenant_id FK`, `document_id FK`, `schema_key`, `value_text`, `value_number`, `value_date`, `confidence` FLOAT, `revision` INT, `created_by` (system|user), `metadata`  
  *Indexes*: (`document_id`, `schema_key`), (`tenant_id`, `schema_key`)

### 12.5 Tasks, Checklists & Schedules
- **checklist_template** (AI or manual source)  
  `id`, `tenant_id FK`, `scope` (enum: org, entity, person), `name`, `version`, `source_type` (enum: system, ai, custom), `jurisdiction_country` NULL, `jurisdiction_region` NULL, `applies_to JSONB` (conditions), `metadata`
- **task_template**  
  `id`, `tenant_id FK`, `checklist_template_id FK`, `name`, `description_md`, `default_due_offset_days` INT NULL, `recurrence_rule` (RFC5545 text) NULL, `priority` (low|normal|high), `requires_evidence` BOOL, `metadata`
- **checklist_instance**  
  `id`, `tenant_id FK`, `org_id FK NULL`, `entity_id FK NULL`, `person_id FK NULL`, `template_id FK NULL`, `name`, `status` (draft|active|archived), `created_from` (enum: ai, template, manual), `metadata`
- **task**  
  `id`, `tenant_id FK`, `checklist_instance_id FK NULL`, `org_id FK NULL`, `entity_id FK NULL`, `name`, `status` (todo|in_progress|blocked|done|skipped), `priority`, `assignee_person_id FK NULL`, `due_at` NULL, `started_at` NULL, `completed_at` NULL, `recurrence_rule` NULL, `source_task_template_id FK NULL`, `evidence_document_id FK NULL`, `ai_summary` TEXT NULL, `metadata`  
  *Indexes*: (`tenant_id`, `status`, `due_at`), (`tenant_id`, `assignee_person_id`)
- **reminder**  
  `id`, `tenant_id FK`, `task_id FK`, `channel` (email|inapp|webhook), `scheduled_at`, `sent_at` NULL, `status` (scheduled|sent|failed), `metadata`

### 12.6 Reporting (MVP‑friendly)
- **materialized views** (per tenant with RLS):  
  - `mv_task_summary`: counts by status, overdue flags, next 30/60/90.  
  - `mv_entity_rollup`: entity counts by type, active/inactive, ownership graph degree.  
  - `mv_activity_feed`: recent actions for dashboards.
- **snapshot** (optional for BI)  
  `id`, `tenant_id`, `as_of_date`, `kpis JSONB` (e.g., `{"tasks_overdue":12,"entities_active":9}`)

### 12.7 AI Layer (Data Artifacts)
- **ai_session**  
  `id`, `tenant_id FK`, `org_id/entity_id/person_id` NULL, `initiated_by_person_id`, `channel` (chat|inline_action|api), `model`, `prompt_preview`, `started_at`, `ended_at`, `metadata`
- **ai_action_log** (tool calls & results)  
  `id`, `tenant_id FK`, `ai_session_id FK`, `action_type` (create_entity|create_task|summarize|extract|classify|other), `request_json`, `response_json`, `latency_ms`, `success` BOOL, `error_msg` NULL
- **feedback**  
  `id`, `tenant_id FK`, `ai_session_id FK NULL`, `target_type` (task|checklist|document|extraction|summary), `target_id`, `rating` (1‑5), `comment`, `created_by`

### 12.8 Security & Access Control
- **role**  
  `id`, `tenant_id FK`, `key` (admin|manager|member|viewer|advisor|auditor), `name`, `description`
- **permission**  
  `id`, `key` (e.g., `entity.read`, `entity.write`, `task.manage`, `report.view`), `description`
- **role_permission**  
  `id`, `tenant_id FK`, `role_key`, `permission_key`
- **person_role_override** (fine‑grained exceptions)  
  `id`, `tenant_id FK`, `person_id FK`, `permission_key`, `allow` BOOL

### 12.9 Auditing
- **audit_log**  
  `id`, `tenant_id FK`, `actor_person_id NULL`, `action` (crud verbs, auth events, ai events), `target_table`, `target_id`, `changes JSONB` (from/to), `ip`, `user_agent`, `occurred_at`

### 12.10 Enumerations (initial)
- `entity_type` = {llc, c_corp, s_corp, lp, llp, trust, sole_prop, plc, non_profit, spv, other}  
- `ownership_model` = {member_managed, manager_managed, board_managed, trustee_managed}  
- `task_status` = {todo, in_progress, blocked, done, skipped}  
- `priority` = {low, normal, high}  
- `document.category` = {formation, compliance, tax, contract, id, other}

### 12.11 Indexing & Performance (starter set)
- Composite indexes on `(tenant_id, …)` for all access paths.  
- GIN on `metadata` JSONB where used for dynamic filters.  
- Partial index for overdue tasks: `CREATE INDEX ON task (tenant_id, due_at) WHERE status <> 'done' AND due_at IS NOT NULL;`  
- Foreign keys declared `ON DELETE RESTRICT` except soft‑deleted records; consider `ON DELETE SET NULL` for optional relations.

### 12.12 Row‑Level Security (high‑level policy outline)
- Enable RLS on all tables.  
- Security context via `SET app.current_person_id`, `SET app.current_tenant_id`.  
- Example policy (read): person can read rows where `tenant_id = current_tenant` AND (is admin OR is member via `org_member` or has explicit `person_role_override`).  
- Separate policies for system workers (queue processors) using dedicated roles.

### 12.13 Soft Delete & Retention
- Soft delete with `is_deleted` boolean + `deleted_at`.  
- Background job to purge documents & PII after retention window (configurable per tenant).

### 12.14 Migration & Seed
- Migration tool: Flyway or Prisma Migrate.  
- Seed minimal roles/permissions, a demo tenant, and 2 example checklist templates (state filing + annual license renewal).

