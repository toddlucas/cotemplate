/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { OrganizationMemberRole } from "./organization-member-role";

export interface OrganizationMemberModel {
    id: number;
    orgId: number;
    personId: number;
    roleId: OrganizationMemberRole;
    status?: string;
    startAt?: Date;
    endAt?: Date;
    metadata?: string;
}
