import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { promise } from 'protractor';
import { Observable } from 'rxjs';
import { GlobalSettingsService } from '../global-settings.service';
import { AuthorizedUser } from '../models/authorized-user';
import { BaseWebApiService } from './base-web-api.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService extends BaseWebApiService {

  private api = 'api/authenticate/';
  constructor(public globalSettings: GlobalSettingsService,
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
