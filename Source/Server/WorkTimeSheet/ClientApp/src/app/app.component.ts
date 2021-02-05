import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { GlobalSettings } from "./models/global-settings";
import { AuthenticationService } from "./services/authentication.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  title = 'app';

  constructor(public globalSettings: GlobalSettings, public router: Router, public authenticationService: AuthenticationService) {

  }

  ngOnInit(): void {
    const authorizedUser = localStorage.getItem('authorizedUser');
    if (authorizedUser == null) {
      this.globalSettings.isLoggedIn = false;
      this.globalSettings.authorizedUser = null;
      this.router.navigate(['/']);
    }
    else {
      this.authenticationService
        .getAccessTokenFromRequestToken(JSON.parse(authorizedUser))
        .subscribe(response => {
          this.globalSettings.isLoggedIn = true;
          this.globalSettings.authorizedUser = response;
          localStorage.setItem('authorizedUser', JSON.stringify(response));
          if (this.router.url == "/")
            this.router.navigate(['/dashboard']);
        }, error => {
          this.router.navigate(['/']);
        });
    }
  }

  public isLoggedIn(): boolean {
    return this.globalSettings.isLoggedIn;
  }
}
