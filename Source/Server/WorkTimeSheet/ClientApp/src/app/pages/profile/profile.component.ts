import { Clipboard } from '@angular/cdk/clipboard';
import { Component, OnInit } from '@angular/core';
import { GlobalSettings } from 'src/app/models/global-settings';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  constructor(public globalSettings: GlobalSettings, private clipboard: Clipboard) { }

  ngOnInit(): void {
  }

  public copyAccessTokenToClipboard(): void {
    this.clipboard.copy(this.globalSettings.authorizedUser.accessToken);
  }
}
