# AI‑First System Diagram (High‑Level Flow)

```
[User Uploads/Connects]
      |  (PDFs, Emails, Drive/Dropbox, Plaid/QBO, E‑sign)
      v
+------------------------------+
|  Ingestion Gateway           |
|  - File/Connector adapters   |
|  - Webhooks (e‑sign)         |
+------------------------------+
               |
               v
+------------------------------+        +--------------------------+
|  Pre‑Process                 |        |  Classification Catalog  |
|  - OCR / Layout              |<------>|  (SAFE, OA, Policy, etc.)|
|  - De‑dup / Virus scan       |        +--------------------------+
+------------------------------+
               |
               v
+------------------------------+        +--------------------------+
|  Extraction                  | -----> |  Confidence & QA Queue   |
|  - LLM + templates/regex     |        |  (Human review if low)   |
|  - Key fields (dates, %s)    |        +--------------------------+
+------------------------------+
               |
               v
+------------------------------+        +--------------------------+
|  Normalization & Linking     |<-----> |  Knowledge Graph (Core)  |
|  - Units/currency/dates      |        |  Entities, People,       |
|  - Map to schema + edges     |        |  Ownership, Obligations  |
+------------------------------+        +--------------------------+
               |                                        ^
               |                                        |
               v                                        |
+------------------------------+                        |
|  Obligation/Task Generator   |------------------------+
|  - Jurisdiction calendars    |  creates/updates tasks
|  - Rules (renewals, filings) |
+------------------------------+
               |
               v
+------------------------------+        +--------------------------+
|  Copilot Orchestration       |<-----> |  Deterministic Tools     |
|  - Skills registry           |        |  (Cap table math,        |
|  - Context builder           |        |   Consolidation engine,  |
|  - Policy/guardrails         |        |   Scenario engine)       |
+------------------------------+        +--------------------------+
               |
               v
+------------------------------+        +--------------------------+
|  Drafts & Explanations       |<-----> |  Provenance & Audit Log  |
|  - Resolutions, Minutes      |        |  (inputs, citations,     |
|  - Reports, Summaries        |        |   hashes, timestamps)    |
+------------------------------+        +--------------------------+
               |
               v
+------------------------------+        +--------------------------+
|  Human Review UI             |<-----> |  Permissions/ACL         |
|  - Side‑by‑side diffs        |        |  (Org, entity, role)     |
|  - Accept/Edit/Reject        |        +--------------------------+
+------------------------------+
               |
               v
+------------------------------+
|  Finalize & Sync             |
|  - Commit to graph           |
|  - Sync to connectors (QBO,  |
|    storage, cap table, etc.) |
+------------------------------+
```

---

## Component Interaction (Swimlane‑style)

```
User     Ingestion   Extract/Normalize   Graph/API     Copilot/Tools   UI
 |           |              |               |              |           |
 | Upload -->|              |               |              |           |
 |           |--Store blob->|               |              |           |
 |           |--Classify--->|               |              |           |
 |           |              |--Fields------>|              |           |
 |           |              |--Edges------->|              |           |
 |           |              |               |--CreateTasks>|           |
 |           |              |               |<--Task IDs---|           |
 |           |              |               |              |           |
 | Ask NL Q ------------------------------------------------>          |
 |           |              |               |--Context---->|           |
 |           |              |               |              |--Compute->|
 |           |              |               |<--Answer/Draft-----------|
 |           |              |               |              |           |
 | Review/Edit ------------------------------------------------------->|
 |           |              |               |<--Commit/Sync------------|
```

---

## Data Provenance Loop

```
[Draft Output]
   | includes
   v
[Citations (doc id, page, field)]
   | back‑references
   v
[Audit Log Entry] ----> [Deterministic Recompute] -> should match -> [Accepted Record]
```

---

### Notes
- Deterministic tools handle math & rules; LLMs handle language, extraction, and explanation.
- Everything important is versioned (docs, graph edges, tasks) for point‑in‑time queries and rollbacks.
- Permissions are enforced in the Context Builder so AI only sees what the user is allowed to see.

