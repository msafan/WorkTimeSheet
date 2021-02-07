import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { FilterModel } from "../models/filter-model";
import { GlobalSettings } from "../models/global-settings";
import { Pagination } from "../models/pagination";

@Injectable({
  providedIn: 'root'
})
export class BaseWebApiService {

  constructor(public globalSettings: GlobalSettings,
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

  protected get<T>(url: string, searchparam?: FilterModel): Observable<T> {
    let search: any = {};
    if (searchparam != null)
      search = searchparam;
    return this.httpClient.get<T>(this.baseUrl + url, { headers: this.getHeader(), params: search });
  }

  protected getPaginated<T>(url: string, searchparam?: Pagination): Observable<T> {
    let search: any = {};
    if (searchparam != null)
      search = searchparam;
    return this.httpClient.get<T>(this.baseUrl + url, { headers: this.getHeader(), params: search });
  }

  protected post<T>(url: string, parameters: any): Observable<T> {
    return this.httpClient.post<T>(this.baseUrl + url, parameters, { headers: this.getHeader() });
  }

  protected put<T>(url: string, parameters: any): Observable<T> {
    return this.httpClient.put<T>(this.baseUrl + url, parameters, { headers: this.getHeader() });
  }

  protected patch<T>(url: string, parameters: any): Observable<T> {
    return this.httpClient.patch<T>(this.baseUrl + url, parameters, { headers: this.getHeader() });
  }

  protected delete<T>(url: string): Observable<T> {
    return this.httpClient.delete<T>(this.baseUrl + url, { headers: this.getHeader() });
  }
}
