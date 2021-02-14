import { Clipboard } from '@angular/cdk/clipboard';
import { Component, OnInit } from '@angular/core';
import { AccessTokenModel } from 'src/app/models/access-token.model';
import { GlobalSettings } from 'src/app/models/global-settings';
import { AccessTokenService } from 'src/app/services/access-token.service';
import { CommonService } from 'src/app/services/common.service';
import { BaseComponent } from '../base-component';

@Component({
  selector: 'app-access-tokens',
  templateUrl: './access-tokens.component.html',
  styleUrls: ['./access-tokens.component.css']
})
export class AccessTokensComponent extends BaseComponent implements OnInit {
  public appName: string = "";
  public accessTokens: AccessTokenModel[] = [];

  constructor(globalSettings: GlobalSettings, commonService: CommonService,
    private clipboard: Clipboard, private accessTokenService: AccessTokenService) {
    super(globalSettings, commonService);
  }

  ngOnInit(): void {
    this.fetchAllAccessTokens();
  }

  public addNewApp(): void {
    if (this.appName == undefined || this.appName == "") {
      this.showError('Error', 'App name cannot be left blank');
      return;
    }

    const accessTokenModel = new AccessTokenModel();
    accessTokenModel.appName = this.appName;
    this.accessTokenService.createNewApp(accessTokenModel).subscribe(result => {
      this.showSuccess('App Created', 'App: ' + this.appName + ' created sucessfully')
      this.appName = "";
      this.closeModal();
      this.fetchAllAccessTokens();
    }, error => {
      this.showException('Error', error);
    });
  }

  public copyToClipboard(value: string): void {
    this.clipboard.copy(value);
    this.showSuccess('Copied', 'Api key copied to clipboard');
  }

  private fetchAllAccessTokens(): void {
    this.accessTokenService.getAll().subscribe(result => {
      this.accessTokens = result;
    }, error => {
      this.showException('Error', error);
    });
  }
}
