import { Component, OnInit } from "@angular/core";
import { GlobalSettings } from "src/app/models/global-settings";
import { BaseComponent } from "src/app/pages/base-component";
import { CommonService } from "src/app/services/common.service";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent extends BaseComponent implements OnInit {
  public userName: string;
  constructor(globalSettings: GlobalSettings, commonService: CommonService) {
    super(globalSettings, commonService);
    this.userName = globalSettings.authorizedUser.name;
  }

  ngOnInit() {
  }

}
