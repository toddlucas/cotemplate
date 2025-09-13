/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { EntityRoleType } from "./entity-role-type";

export interface EntityRoleModel {
    id: number;
    orgId: number;
    entityId: number;
    personId: number;
    roleId: EntityRoleType;
    equityPercent?: number;
    unitsShares?: number;
    startAt?: Date;
    endAt?: Date;
    metadata?: string;
}
