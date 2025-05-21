import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { ApiResponse } from '../models/api-response.model';
import { Projects } from '../models/project.model';
import { envirovment } from '../../../../environments/environment';
import { Observable } from 'rxjs/internal/Observable';
import { catchError, map, throwError } from 'rxjs';

interface GetProjectsResponse {
  projects: Projects[];
}

@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${envirovment.projectApiBaseUrl}/projects`;

  constructor() {}

  getProjects(): Observable<Projects[]> {
    return this.http
      .get<ApiResponse<GetProjectsResponse>>(this.apiUrl) // Expect ApiResponse containing GetProjectsResponse
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
            const structureErrorMsg =
              'Unexpected response structure from API when fetching projects.';
            throw new Error(structureErrorMsg);
          }
          const structureErrorMsg =
            'Unexpected response structure from API when fetching projects.';
          throw new Error(structureErrorMsg);
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
