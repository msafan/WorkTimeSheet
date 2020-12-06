import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GlobalSettingsService } from '../global-settings.service';
import { OrganizationModel } from '../models/organization-model';
import { RegistrationModel } from '../models/registration-model';
import { BaseWebApiService } from './base-web-api.service';

@Injectable({
  providedIn: 'root'
})
export class OrganizationService extends BaseWebApiService {

  private api = 'api/organization/';

  constructor(public globalSettings: GlobalSettingsService,
    public httpClient: HttpClient,
    @Inject('BASE_URL') public baseUrl: string) {
    super(globalSettings, httpClient, baseUrl);
  }

  public register(registrationModel: RegistrationModel): Observable<OrganizationModel> {
    return this.post<OrganizationModel>(this.api + 'register', registrationModel);
  }
}
