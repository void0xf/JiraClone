import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { ApiResponse } from '../models/api-response.model';
import { Project } from '../models/project.model';
import { envirovment } from '../../../../environments/environment';
import { Observable } from 'rxjs/internal/Observable';
import { catchError, map, throwError } from 'rxjs';

interface GetProjectResponse {
  projects: Project[];
}

@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${envirovment.projectApiBaseUrl}/projects`;

  constructor() {}

  getProjects(): Observable<Project[]> {
    return this.http
      .get<ApiResponse<GetProjectResponse>>(this.apiUrl) // Expect ApiResponse containing GetProjectResponse
      .pipe(
        map((response) => {
          if (
            response &&
            response.data &&
            Array.isArray(response.data.projects)
          ) {
            return response.data.projects;
          }
          if (!response.isSuccess) {
            const structureErrorMsg = `Unexpected response structure from API when fetching Project. ${response}`;
            throw new Error(structureErrorMsg);
          }
          const structureErrorMsg = `Unexpected response structure from API when fetching Project. ${response}`;
          throw new Error(structureErrorMsg);
        }),
        catchError(this.handleError)
      );
  }

  getProjectByKey(projectKey: string): Observable<Project> {
    return this.http
      .get<ApiResponse<Project>>(`${this.apiUrl}/${projectKey}`)
      .pipe(
        map((response) => {
          if (response && response.isSuccess && response.data) {
            return response.data;
          }
          let errorMsg = `Error fetching project with key ${projectKey}.`;
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
    return throwError(errorMessage);
  }
}
