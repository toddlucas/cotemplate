/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { Priority } from "./priority";

export interface TaskTemplateModel {
    id: number;
    checklistTemplateId: number;
    name: string;
    descriptionMd?: string;
    defaultDueOffsetDays?: number;
    recurrenceRule?: string;
    priorityId?: Priority;
    requiresEvidence: boolean;
    metadata?: string;
}
