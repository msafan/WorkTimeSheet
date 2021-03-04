import { Component, OnInit } from '@angular/core';
import { GlobalSettings } from 'src/app/models/global-settings';
import { CommonService } from 'src/app/services/common.service';
import { BaseComponent } from '../base-component';

@Component({
  selector: 'app-mobile-apps',
  templateUrl: './mobile-apps.component.html',
  styleUrls: ['./mobile-apps.component.css']
})
export class MobileAppsComponent extends BaseComponent implements OnInit {

  constructor(globalSettings: GlobalSettings, commonService: CommonService) {
    super(globalSettings, commonService);
  }

  ngOnInit(): void {
  }

  public downloadApk(): void {
    this.commonService.getApkFile().subscribe(
      result => {
        this.commonService.downloadFile(result, "arraybuffer", "WorkTimeSheet.apk");
      }, error => {
        this.showException('Error', error);
      });
  }
}
