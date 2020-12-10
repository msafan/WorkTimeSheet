import { Component, OnInit } from "@angular/core";
import { GlobalSettings } from "src/app/models/global-settings";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  public userName: string;
  constructor(globalSettings: GlobalSettings) {
    this.userName = globalSettings.authorizedUser.name;
  }

  ngOnInit() {
  }

}
