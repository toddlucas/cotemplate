/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { ChecklistStatus } from "./checklist-status";
import { SourceType } from "./source-type";

export interface ChecklistModel {
    id: number;
    tenantId: number;
    orgId: number;
    entityId?: number;
    personId?: number;
    templateId?: number;
    name: string;
    statusId: ChecklistStatus;
    createdFromId: SourceType;
    metadata?: string;
}
