import { Component, OnInit } from "@angular/core";
import { GlobalSettings } from "src/app/models/global-settings";
import { BaseComponent } from "src/app/pages/base-component";

@Component({
  selector: 'app-side-bar',
  templateUrl: './side-bar.component.html',
  styleUrls: ['./side-bar.component.css']
})
export class SideBarComponent extends BaseComponent implements OnInit {

  constructor(globalSettings: GlobalSettings) {
    super(globalSettings);
}

  ngOnInit() {
  }

}