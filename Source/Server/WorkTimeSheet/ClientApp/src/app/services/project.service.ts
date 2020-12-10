import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GlobalSettings } from '../models/global-settings';
import { PaginatedResults } from '../models/paginated-results';
import { Pagination } from '../models/pagination';
import { ProjectFilterModel } from '../models/project-filter-model';
import { ProjectModel } from '../models/project-model';
import { UserModel } from '../models/user-model';
import { BaseWebApiService } from './base-web-api.service';

@Injectable({
  providedIn: 'root'
})
export class ProjectService extends BaseWebApiService {
  private api = 'api/project/';

  constructor(globalSettings: GlobalSettings,
    public httpClient: HttpClient,
    @Inject('BASE_URL') public baseUrl: string) {
    super(globalSettings, httpClient, baseUrl);
  }

  public getAll(projectFilterModel: ProjectFilterModel): Observable<PaginatedResults<ProjectModel>> {
    return this.get<PaginatedResults<ProjectModel>>(this.api, projectFilterModel);
  }

  public create(projectModel: ProjectModel): Observable<ProjectModel> {
    return this.post<ProjectModel>(this.api, projectModel);
  }

  public getById(id: Number): Observable<ProjectModel> {
    return this.get<ProjectModel>(this.api + id);
  }

  public edit(id: number, projectName: ProjectModel): Observable<ProjectModel> {
    return this.put<ProjectModel>(this.api + id, projectName);
  }

  public getAllMembers(projectId: number): Observable<PaginatedResults<UserModel>> {
    return this.get<PaginatedResults<UserModel>>(this.api + "members/" + projectId);
  }

  public updateMembers(projectId: number, userIds: number[]): Observable<PaginatedResults<UserModel>> {
    return this.put<PaginatedResults<UserModel>>(this.api + "updatemembers/" + projectId, userIds);
  }
}
