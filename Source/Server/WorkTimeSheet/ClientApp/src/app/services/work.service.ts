import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CurrentWork } from '../models/current-work';
import { GlobalSettings } from '../models/global-settings';
import { BaseWebApiService } from './base-web-api.service';

@Injectable({
  providedIn: 'root'
})
export class WorkService extends BaseWebApiService {
  private api = 'api/work/';

  constructor(public globalSettings: GlobalSettings,
    public httpClient: HttpClient,
    @Inject('BASE_URL') public baseUrl: string) {
    super(globalSettings, httpClient, baseUrl);
  }

  public GetCurrentWork(): Observable<CurrentWork> {
    return this.get<CurrentWork>(this.api);
  }

  public startWork(projectId: number): Observable<any> {
    return this.patch(this.api + "start/" + projectId, {});
  }

  public stopWork(remarks: string): Observable<any> {
    return this.patch(this.api + "stop/" + remarks, {});
  }
}
