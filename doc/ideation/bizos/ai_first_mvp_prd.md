# Product Requirements Document (PRD)
**Product**: [Working Title – e.g. “Core Platform”]  
**Version**: 0.1 (Draft)  
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

## 4. User Stories
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

## 5. Functional Requirements
- CRUD for organizations, entities, people/advisors.  
- Role-based access control with per-org permissions.  
- Task/checklist management (AI-generated, editable).  
- Notifications/reminders (email in MVP).  
- Reporting with drill-downs (entity → org → portfolio).  
- AI assistant integrated across modules (chat-based + contextual actions).  

---

## 6. AI-First Design Principles
- **Default entry point**: chat/assistant guides setup & usage.  
- **Human-in-loop**: all AI-generated items are editable before being saved.  
- **Feedback loop**: user edits logged to improve prompt templates/models.  
- **Transparency**: AI outputs show source templates/rules when applicable.  

---

## 7. UX/UI Requirements
- Clean, modular dashboard (React + Tailwind).  
- AI assistant accessible from every page.  
- Entity/people/task lists with search & filters.  
- Reporting dashboard with AI “summary” button.  

---

## 8. Technical Requirements
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

## 9. Success Metrics
- **Adoption**: 10 pilot orgs onboarded within first 90 days.  
- **Engagement**: 70%+ of tasks created via AI assistant.  
- **Retention**: 60% of orgs active at 90 days.  
- **Accuracy**: <15% of AI-generated checklist items marked as irrelevant.  

---

## 10. Risks & Mitigations
- **AI hallucinations** → Constrain outputs with templates + review.  
- **Security concerns** → Row-level security + role-based access.  
- **Overgeneralization** → Focus MVP on 1-2 niches (e.g., wealth managers + real estate).  
- **Adoption friction** → Emphasize natural language setup vs manual form filling.  

---

## 11. Open Questions
- Which niche to target first: wealth managers, family office light, or real estate funds?  
- Should AI assistant be embedded as chat only, or also inline (e.g., “AI suggest” buttons)?  
- To what extent should we allow white-label branding in MVP?

