import { Clipboard } from '@angular/cdk/clipboard';
import { Component, OnInit } from '@angular/core';
import { GlobalSettings } from 'src/app/models/global-settings';
import { CommonService } from 'src/app/services/common.service';
import { BaseComponent } from '../base-component';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent extends BaseComponent implements OnInit {

  constructor(public globalSettings: GlobalSettings,
    private clipboard: Clipboard, commonService: CommonService) {
    super(globalSettings, commonService);
  }

  ngOnInit(): void {
  }

  public copyAccessTokenToClipboard(): void {
    this.clipboard.copy(this.globalSettings.authorizedUser.accessToken);
    this.showSuccess('Copied', 'Access token copied to clipboard');
  }
}
