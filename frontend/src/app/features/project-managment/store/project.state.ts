// src/app/features/project/store/project.state.ts
import { EntityState, createEntityAdapter, EntityAdapter } from '@ngrx/entity';
import { Project } from '../../../core/models/project.model';

export interface ProjectState extends EntityState<Project> {
  projects: Project[];
  selectedProject: Project | null;
  loading: boolean;
  error: any | null;
  allProjectsLoaded: boolean;

  currentProject: Project | null;
  currentProjectLoading: boolean;
  currentProjectError: any | null;
}

export const projectAdapter: EntityAdapter<Project> =
  createEntityAdapter<Project>({
    selectId: (project: Project) => project.id,
  });

export const initialProjectState: ProjectState = projectAdapter.getInitialState(
  {
    projects: [],
    selectedProject: null,
    loading: false,
    error: null,
    allProjectsLoaded: false,

    currentProject: null,
    currentProjectLoading: false,
    currentProjectError: null,
  }
);
