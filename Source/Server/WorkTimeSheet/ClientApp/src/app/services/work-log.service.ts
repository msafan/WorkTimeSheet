import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GlobalSettings } from '../models/global-settings';
import { WorkLogFilterModel } from '../models/work-log-filter-model';
import { WorkLogReport } from '../models/work-log-report';
import { BaseWebApiService } from './base-web-api.service';

@Injectable({
  providedIn: 'root'
})
export class WorkLogService extends BaseWebApiService {
  private api = 'api/worklog/';

  constructor(public globalSettings: GlobalSettings,
    public httpClient: HttpClient,
    @Inject('BASE_URL') public baseUrl: string) {
    super(globalSettings, httpClient, baseUrl);
  }

  public getAll(workLogFilterModel: WorkLogFilterModel): Observable<WorkLogReport> {
    return this.get<WorkLogReport>(this.api, workLogFilterModel);
  }
}
