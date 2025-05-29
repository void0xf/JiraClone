// src/app/features/project/store/project.actions.ts
import { createAction, props } from '@ngrx/store';
import { Update } from '@ngrx/entity';
import { Project } from '../../../../core/models/project.model';

// Action to trigger loading all projects
export const loadProjects = createAction('[Project API] Load Projects');

export const loadProjectsSuccess = createAction(
  '[Project API] Load Projects Success',
  props<{ projects: Project[] }>()
);

export const loadProjectsFailure = createAction(
  '[Project API] Load Projects Failure',
  props<{ error: any }>()
);

// Actions for creating a project (example)
export const createProject = createAction(
  '[Create Project Modal] Create Project',
  props<{ projectData: Omit<Project, 'id' | 'createdAt' | 'updatedAt'> }>()
);

export const createProjectSuccess = createAction(
  '[Project API] Create Project Success',
  props<{ project: Project }>()
);

export const createProjectFailure = createAction(
  '[Project API] Create Project Failure',
  props<{ error: any }>()
);

// Action for selecting a project
export const selectProject = createAction(
  '[Project List/Router] Select Project',
  props<{ projectId: string | null }>()
);

// Actions for loading a single project by its key
export const loadSingleProject = createAction(
  '[Project Detail Page] Load Single Project',
  props<{ projectKey: string }>()
);

export const loadSingleProjectSuccess = createAction(
  '[Project API] Load Single Project Success',
  props<{ project: Project }>() // Assuming Project is the detailed project model
);

export const loadSingleProjectFailure = createAction(
  '[Project API] Load Single Project Failure',
  props<{ error: any }>()
);

// Add more actions as needed (update, delete, load single project details if necessary)
