import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ProjectState, projectAdapter } from './project.state';
import { projectFeatureKey } from './project.reducer'; // Corrected import for projectFeatureKey
import { Project } from '../../../core/models/project.model';

// Select the feature state
export const selectProjectState =
  createFeatureSelector<ProjectState>(projectFeatureKey);

// Get the selectors from the adapter
const { selectAll, selectEntities, selectIds, selectTotal } =
  projectAdapter.getSelectors();

// Select all projects
export const selectAllProjects = createSelector(selectProjectState, selectAll);

// Select project entities (dictionary)
export const selectProjectEntities = createSelector(
  selectProjectState,
  selectEntities
);

// Select project IDs
export const selectProjectIds = createSelector(selectProjectState, selectIds);

// Select total number of projects
export const selectProjectTotal = createSelector(
  selectProjectState,
  selectTotal
);

// Select the currently selected project
export const selectSelectedProject = createSelector(
  selectProjectState,
  (state: ProjectState) => state.selectedProject
);

export const selectProjectByKeyValue = (key: keyof Project) => 
  createSelector(
    selectAllProjects,
    (projects: Project[], value: string ) => {
      return projects.find(project => project[key] === value);
    }
  )
export const selectProjectByProjectKey = selectProjectByKeyValue('projectKey'); 
// Select loading status
export const selectProjectsLoading = createSelector(
  selectProjectState,
  (state: ProjectState) => state.loading
);

// Select error
export const selectProjectsError = createSelector(
  selectProjectState,
  (state: ProjectState) => state.error
);

// Select if all projects have been loaded (useful for knowing if initial load is done)
export const selectAllProjectsLoaded = createSelector(
  selectProjectState,
  (state: ProjectState) => state.allProjectsLoaded
);

// Selectors for the detailed project view
export const selectCurrentDetailedProject = createSelector(
  selectProjectState,
  (state: ProjectState) => state.currentProject
);

export const selectCurrentProjectLoading = createSelector(
  selectProjectState,
  (state: ProjectState) => state.currentProjectLoading
);

export const selectCurrentProjectError = createSelector(
  selectProjectState,
  (state: ProjectState) => state.currentProjectError
);
