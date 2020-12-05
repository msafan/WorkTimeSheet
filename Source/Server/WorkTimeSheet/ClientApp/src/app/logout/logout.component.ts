import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalSettingsService } from '../global-settings.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {

  constructor(public globalSettings:GlobalSettingsService, public router: Router) { }

  ngOnInit() {
    this.globalSettings.isLoggedIn = false;
    this.router.navigate(['/']);
  }

}
