/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { TaskStatus } from "./task-status";
import { Priority } from "./priority";

export interface TaskModel {
    id: number;
    tenantId: number;
    orgId: number;
    checklistId?: number;
    entityId?: number;
    name: string;
    statusId: TaskStatus;
    priorityId?: Priority;
    assigneePersonId?: number;
    dueAt?: Date;
    startedAt?: Date;
    completedAt?: Date;
    recurrenceRule?: string;
    sourceTaskTemplateId?: number;
    evidenceDocumentId?: number;
    aiSummary?: string;
    metadata?: string;
}
