import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { GlobalSettings } from "../models/global-settings";
import { OrganizationModel } from "../models/organization-model";
import { RegistrationModel } from "../models/registration-model";
import { BaseWebApiService } from "./base-web-api.service";

@Injectable({
  providedIn: 'root'
})
export class OrganizationService extends BaseWebApiService {

  private api = 'api/organization/';

  constructor(globalSettings: GlobalSettings,
    public httpClient: HttpClient,
    @Inject('BASE_URL') public baseUrl: string) {
    super(globalSettings, httpClient, baseUrl);
  }

  public register(registrationModel: RegistrationModel): Observable<OrganizationModel> {
    return this.post<OrganizationModel>(this.api + 'register', registrationModel);
  }

  public getMyOrganization(): Observable<OrganizationModel> {
    return this.get<OrganizationModel>(this.api + "myorganization");
  }

  public edit(id: number, organizationModel: OrganizationModel) {
    // return this.put<OrganizationModel>(this.api + id, organizationModel);
    return this.post<OrganizationModel>(this.api + id, organizationModel);
  }
}
