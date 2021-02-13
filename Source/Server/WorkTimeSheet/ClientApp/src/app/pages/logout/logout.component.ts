import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalSettings } from 'src/app/models/global-settings';
import { CommonService } from 'src/app/services/common.service';
import { BaseComponent } from '../base-component';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent extends BaseComponent implements OnInit {

  constructor(private globalSettings: GlobalSettings, private router: Router,
    commonService: CommonService) {
    super(globalSettings, commonService);
  }

  ngOnInit() {
    this.globalSettings.isLoggedIn = false;
    this.globalSettings.authorizedUser = null;
    localStorage.removeItem('authorizedUser');
    this.router.navigate(['/']);
  }

}
