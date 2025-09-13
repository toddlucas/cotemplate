/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { EntityType } from "./entity-type";
import { OwnershipModel } from "./ownership-model";
import { EntityStatus } from "./entity-status";

export interface EntityModel {
    id: number;
    orgId: number;
    name: string;
    legalName?: string;
    entityTypeId: EntityType;
    formationDate?: Date;
    jurisdictionCountry?: string;
    jurisdictionRegion?: string;
    ein?: string;
    stateFileNumber?: string;
    registeredAgent?: string;
    ownershipModelId?: OwnershipModel;
    statusId: EntityStatus;
    metadata?: string;
}
