import { Component, OnInit } from '@angular/core';
import { GlobalSettingsService } from './global-settings.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  title = 'app';

  constructor(public globalSettings: GlobalSettingsService) {

  }
  
  ngOnInit(): void {
    this.globalSettings.isLoggedIn = false;
  }

  public isLoggedIn(): boolean {
    return this.globalSettings.isLoggedIn;
  }
}
