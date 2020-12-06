import { Injectable } from '@angular/core';
import { AuthorizedUser } from './models/authorized-user';

@Injectable({
  providedIn: 'root'
})
export class GlobalSettingsService {

  constructor() { }

  public isLoggedIn: boolean = false;

  public authorizedUser: AuthorizedUser;
}
