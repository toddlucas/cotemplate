/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { DocumentCategory } from "./document-category";

export interface DocumentModel {
    id: number;
    orgId: number;
    entityId?: number;
    personId?: number;
    title: string;
    categoryId: DocumentCategory;
    storageUri?: string;
    mimeType?: string;
    hash?: string;
    uploadedBy?: number;
    uploadedAt?: Date;
    metadata?: string;
}
