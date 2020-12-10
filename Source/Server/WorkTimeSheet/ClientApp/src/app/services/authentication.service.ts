import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AuthorizedUser } from "../models/authorized-user";
import { GlobalSettings } from "../models/global-settings";
import { BaseWebApiService } from "./base-web-api.service";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService extends BaseWebApiService {

  private api = 'api/authenticate/';
  constructor(globalSettings: GlobalSettings,
    public httpClient: HttpClient,
    @Inject('BASE_URL') public baseUrl: string) {
    super(globalSettings, httpClient, baseUrl);
  }

  public getAccessTokenFromRequestToken(authorizedUser: AuthorizedUser): Observable<AuthorizedUser> {
    return this.post<AuthorizedUser>(this.api + 'refresh', authorizedUser);
  }

  public login(email: string, password: string): Observable<AuthorizedUser> {
    return this.post<AuthorizedUser>(this.api, { email: email, password: password });
  }
}
