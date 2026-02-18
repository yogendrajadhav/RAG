export interface SourceDocument {
    fileName: string;
    content: string;
    pageNumber:string;
    score:string;
}

export interface UploadResponse{
    success: boolean;
    message: string;
    documentId: string;
    chunksProcessed:string;
}

export interface QueryRequest {
    query: string;
    topK: number;
}

export interface QueryResponse {
    answer: string;
    sources: SourceDocument[];
    processingTimeMs: string;
}

