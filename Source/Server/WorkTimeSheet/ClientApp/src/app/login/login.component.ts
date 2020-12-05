import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalSettingsService } from '../global-settings.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(public globalSettings: GlobalSettingsService, public router: Router) { }

  ngOnInit() {
    this.globalSettings.isLoggedIn=false;
  }

  public login(): void {
    this.globalSettings.isLoggedIn = true;
    this.router.navigate(['/dashboard']);
  }

}
