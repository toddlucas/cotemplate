/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { EntityRelationshipType } from "./entity-relationship-type";

export interface EntityRelationshipModel {
    id: number;
    orgId: number;
    parentEntityId: number;
    childEntityId: number;
    relationshipTypeId: EntityRelationshipType;
    percentOwnership?: number;
    startAt?: Date;
    endAt?: Date;
    metadata?: string;
}
