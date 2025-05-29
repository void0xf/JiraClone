// src/app/features/project/store/project.reducer.ts
import { createReducer, on } from '@ngrx/store';
import {
  ProjectState,
  initialProjectState,
  projectAdapter,
} from './project.state';
import * as ProjectActions from './actions/project.actions';

export const projectFeatureKey = 'projects';

export const projectReducer = createReducer(
  initialProjectState,

  on(ProjectActions.loadProjects, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),

  on(ProjectActions.loadProjectsSuccess, (state, { projects }) => {
    return projectAdapter.setAll(projects, {
      ...state,
      loading: false,
      allProjectsLoaded: true,
    });
  }),

  on(ProjectActions.loadProjectsFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
    allProjectsLoaded: false,
  })),

  on(ProjectActions.createProject, (state) => ({
    ...state,
    loading: true,
  })),

  on(ProjectActions.createProjectSuccess, (state, { project }) => {
    return projectAdapter.addOne(project, {
      ...state,
      loading: false,
    });
  }),

  on(ProjectActions.createProjectFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  })),

  on(ProjectActions.selectProject, (state, { projectId }) => {
    if (!projectId) {
      return {
        ...state,
        selectedProject: null,
      };
    }
    const selectedProject = state.entities[projectId] || null;
    return {
      ...state,
      selectedProject: selectedProject,
    };
  }),

  on(ProjectActions.loadSingleProject, (state) => ({
    ...state,
    currentProjectLoading: true,
    currentProjectError: null,
  })),

  on(ProjectActions.loadSingleProjectSuccess, (state, { project }) => ({
    ...state,
    currentProject: project,
    currentProjectLoading: false,
    // Optional: Add/update this project in the main entity collection if it makes sense
    // ... projectAdapter.upsertOne(project, state) ...
  })),

  on(ProjectActions.loadSingleProjectFailure, (state, { error }) => ({
    ...state,
    currentProjectLoading: false,
    currentProjectError: error,
  }))
);
