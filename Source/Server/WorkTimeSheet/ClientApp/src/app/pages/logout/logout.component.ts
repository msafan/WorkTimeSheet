import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalSettings } from 'src/app/models/global-settings';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {

  constructor(private globalSettings: GlobalSettings, private router: Router) { }

  ngOnInit() {
    this.globalSettings.isLoggedIn = false;
    this.globalSettings.authorizedUser = null;
    localStorage.removeItem('authorizedUser');
    this.router.navigate(['/']);
  }

}
