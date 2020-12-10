import { Injectable } from "@angular/core";
import { AuthorizedUser } from "./authorized-user";

@Injectable({ providedIn: 'root' })
export class GlobalSettings {
    public isLoggedIn: boolean = false;
    public authorizedUser: AuthorizedUser = null;
}
