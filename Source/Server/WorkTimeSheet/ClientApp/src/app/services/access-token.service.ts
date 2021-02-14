import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AccessTokenModel } from '../models/access-token.model';
import { GlobalSettings } from '../models/global-settings';
import { BaseWebApiService } from './base-web-api.service';

@Injectable({
  providedIn: 'root'
})
export class AccessTokenService extends BaseWebApiService {

  private api = 'api/accesstoken/';

  constructor(globalSettings: GlobalSettings,
    public httpClient: HttpClient,
    @Inject('BASE_URL') public baseUrl: string) {
    super(globalSettings, httpClient, baseUrl);
  }

  public getAll(): Observable<AccessTokenModel[]> {
    return this.get<AccessTokenModel[]>(this.api);
  }

  public createNewApp(accessTokenModel: AccessTokenModel): Observable<AccessTokenModel> {
    return this.post<AccessTokenModel>(this.api, accessTokenModel);
  }

  public deleteApp(id: number): Observable<any> {
    return this.delete<any>(this.api + id);
  }
}
