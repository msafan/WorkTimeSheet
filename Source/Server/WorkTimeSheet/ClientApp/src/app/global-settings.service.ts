import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GlobalSettingsService {

  constructor() { }

  public isLoggedIn: boolean = false;
}
