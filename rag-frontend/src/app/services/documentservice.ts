import { Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';
import { QueryResponse, UploadResponse } from '../models/document';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class DocumentService {
  apiUrl= 'http://localhost:5000'; // Update with your backend URL
  constructor(private http: HttpClient) {}

  uploadDocument(file:File):Observable<UploadResponse>{
    const formData = new FormData();
    formData.append('file', file, file.name);
   
   return this.http.post<UploadResponse>(`${this.apiUrl}/upload`, formData).
   pipe(
      // Handle any necessary transformations or error handling here
      // For example, you can map the response to your UploadResponse model
      map((response: any) => {
        return {
          success: response.success,
          message: response.message,
          documentId: response.documentId,
          chunksProcessed: response.chunksProcessed,
        } as UploadResponse;
      }),
      catchError((error) => {
        // Handle errors here, e.g., log them or return a default value
        console.error('Error uploading document:', error);
        return of({
          success: false,
          message: 'Error uploading document',
          documentId: '',
          chunksProcessed: '',
        } as UploadResponse);  
      }));
      }

  queryDocument(query: string, topK: number): Observable<QueryResponse> {
    const requestBody = {
      query: query,
      topK: topK,
    };
    return this.http.post<QueryResponse>(`${this.apiUrl}/query`, requestBody);   
  }
  }

