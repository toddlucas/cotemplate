/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { ChecklistScope } from "./checklist-scope";
import { SourceType } from "./source-type";

export interface ChecklistTemplateModel {
    id: number;
    tenantId: number;
    scopeId: ChecklistScope;
    name: string;
    version?: string;
    sourceTypeId: SourceType;
    jurisdictionCountry?: string;
    jurisdictionRegion?: string;
    appliesTo?: string;
    metadata?: string;
}
