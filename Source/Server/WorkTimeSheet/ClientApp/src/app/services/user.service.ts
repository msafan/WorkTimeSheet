import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateUserModel } from '../models/create-user-model';
import { FilterModel } from '../models/filter-model';
import { GlobalSettings } from '../models/global-settings';
import { PaginatedResults } from '../models/paginated-results';
import { Pagination } from '../models/pagination';
import { UserFilterModel } from '../models/user-filter-model';
import { UserModel } from '../models/user-model';
import { UserRole } from '../models/user-role';
import { BaseWebApiService } from './base-web-api.service';

@Injectable({
  providedIn: 'root'
})
export class UserService extends BaseWebApiService {
  private api = 'api/user/';

  constructor(public globalSettings: GlobalSettings,
    public httpClient: HttpClient,
    @Inject('BASE_URL') public baseUrl: string) {
    super(globalSettings, httpClient, baseUrl);
  }

  public getAll(userFilterModel?: UserFilterModel): Observable<PaginatedResults<UserModel>> {
    return this.get<PaginatedResults<UserModel>>(this.api, userFilterModel);
  }

  public getAllUserRoles(): Observable<UserRole[]> {
    return this.get<UserRole[]>(this.api + 'roles');
  }

  public createUser(createUserModel: CreateUserModel): Observable<UserModel> {
    return this.post<UserModel>(this.api, createUserModel);
  }

  public getById(userId: number): Observable<UserModel> {
    return this.get<UserModel>(this.api + userId);
  }

  public edituser(id: number, userModel: UserModel): Observable<UserModel> {
    // return this.put<UserModel>(this.api + id, userModel);
    return this.post<UserModel>(this.api + id, userModel);
  }

  public deleteUser(userId: number): Observable<any> {
    return this.delete<any>(this.api + userId);
  }
}
