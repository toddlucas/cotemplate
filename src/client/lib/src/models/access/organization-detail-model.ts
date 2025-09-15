/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { ITemporal } from "../i-temporal";
import { OrganizationModel } from "./organization-model";
import { EntityModel } from "../business/entity-model";
import { OrganizationMemberModel } from "./organization-member-model";
import { TaskModel } from "../workflow/task-model";
import { ChecklistModel } from "../workflow/checklist-model";

export interface OrganizationDetailModel extends ITemporal, OrganizationModel {
    entities: EntityModel[];
    members: OrganizationMemberModel[];
    tasks: TaskModel[];
    checklistInstances: ChecklistModel[];
    createdAt: Date;
    updatedAt: Date;
}
