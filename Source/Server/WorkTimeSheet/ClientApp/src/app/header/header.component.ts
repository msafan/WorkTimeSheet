import { Component, OnInit } from '@angular/core';
import { GlobalSettingsService } from '../global-settings.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor(public globalSettings: GlobalSettingsService) { 
    globalSettings.authorizedUser.name
  }

  ngOnInit() {
  }

}
