import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { ApiResponse } from '../models/api-response.model';
import { Issue } from '../../models/issue.model';
import { envirovment } from '../../../../environments/environment';
import { Observable } from 'rxjs/internal/Observable';
import { catchError, map, throwError } from 'rxjs';

interface GetIssuesResponse {
  issues: Issue[];
}

@Injectable({
  providedIn: 'root',
})
export class IssueService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${envirovment.issueApiBaseUrl}/issue`; // Assuming you have an issueApiBaseUrl in your environment config

  constructor() {}

  createIssue(issueData: Issue): Observable<Issue> {
    return this.http
      .post<ApiResponse<Issue>>(this.apiUrl, issueData)
      .pipe(
        map((response) => {
          if (response && response.isSuccess && response.data) {
            return response.data;
          }
          let errorMsg = `Error creating issue.`;
          if (response && !response.isSuccess) {
            errorMsg += ` API indicated failure.`;
          } else if (!response?.data) {
            errorMsg += ` No data returned.`;
          }
          throw new Error(errorMsg);
        }),
        catchError(this.handleError)
      );
  }

  getIssueById(issueId: string): Observable<Issue> {
    return this.http
      .get<ApiResponse<Issue>>(`${this.apiUrl}/${issueId}`)
      .pipe(
        map((response) => {
          if (response && response.isSuccess && response.data) {
            return response.data;
          }
          let errorMsg = `Error fetching issue with ID ${issueId}.`;
          if (response && !response.isSuccess) {
            errorMsg += ` API indicated failure.`;
          } else if (!response?.data) {
            errorMsg += ` No data returned.`;
          }
          throw new Error(errorMsg);
        }),
        catchError(this.handleError)
      );
  }

  getIssuesByProjectId(projectId: string): Observable<Issue[]> {
    return this.http
      .get<ApiResponse<GetIssuesResponse>>(`${this.apiUrl}/project/${projectId}`) // Assuming an endpoint like /issues/project/{projectId}
      .pipe(
        map((response) => {
          if (
            response &&
            response.data &&
            Array.isArray(response.data.issues)
          ) {
            return response.data.issues;
          }
          if (!response.isSuccess) {
            const structureErrorMsg = `Unexpected response structure from API when fetching Issues for project ${projectId}. ${response}`;
            throw new Error(structureErrorMsg);
          }
          const structureErrorMsg = `Unexpected response structure from API when fetching Issues for project ${projectId}. ${response}`;
          throw new Error(structureErrorMsg);
        }),
        catchError(this.handleError)
      );
  }

  updateIssue(issueId: string, issueData: Partial<Issue>): Observable<Issue> {
    return this.http
      .patch<ApiResponse<Issue>>(`${this.apiUrl}/${issueId}`, issueData)
      .pipe(
        map((response) => {
          if (response && response.isSuccess && response.data) {
            return response.data;
          }
          let errorMsg = `Error updating issue with ID ${issueId}.`;
          if (response && !response.isSuccess) {
            errorMsg += ` API indicated failure.`;
          } else if (!response?.data) {
            errorMsg += ` No data returned.`;
          }
          throw new Error(errorMsg);
        }),
        catchError(this.handleError)
      );
  }

  deleteIssue(issueId: string): Observable<void> {
    return this.http
      .delete<ApiResponse<null>>(`${this.apiUrl}/${issueId}`)  
      .pipe(
        map((response) => {
          if (response && response.isSuccess) {
            return; 
          }
          let errorMsg = `Error deleting issue with ID ${issueId}.`;
          if (response && !response.isSuccess) {
            errorMsg += ` API indicated failure.`;
          }
          throw new Error(errorMsg);
        }),
        catchError(this.handleError)
      );
  }

  private handleError(error: HttpErrorResponse) {
    console.error('API error:', error);
    let errorMessage = 'An unknown error occurred.';
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      if (error.status === 404) {
        errorMessage = 'Resource not found.';
      } else if (error.status === 401) {
        errorMessage = 'Unauthorized access.';
      } else {
        errorMessage = `Server returned code ${error.status}, body was: ${error.error}`;
      }
    }
    return throwError(() => new Error(errorMessage));
  }
} 