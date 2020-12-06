import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GlobalSettingsService } from '../global-settings.service';

@Injectable({
  providedIn: 'root'
})
export class BaseWebApiService {

  constructor(public globalSettings: GlobalSettingsService,
    public httpClient: HttpClient,
    @Inject('BASE_URL') public baseUrl: string) { }

  protected getHeader(): HttpHeaders {
    if (this.globalSettings.authorizedUser == null)
      return new HttpHeaders();

    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.globalSettings.authorizedUser.accessToken}`
    });
  }

  protected get<T>(url: string): Observable<T> {
    return this.httpClient.get<T>(this.baseUrl + url, { headers: this.getHeader() });
  }

  protected post<T>(url: string, parameters:any): Observable<T> {
    return this.httpClient.post<T>(this.baseUrl + url, parameters, { headers: this.getHeader() });
  }

  protected put<T>(url: string, parameters:any): Observable<T> {
    return this.httpClient.put<T>(this.baseUrl + url, parameters, { headers: this.getHeader() });
  }

  protected delete<T>(url: string): Observable<T> {
    return this.httpClient.delete<T>(this.baseUrl + url, { headers: this.getHeader() });
  }
}
