import { GlobalSettings } from "../models/global-settings";

export class BaseComponent {
    public isOwner: boolean = false;
    public isProjectManager: boolean = false;
    constructor(globalSettings: GlobalSettings) {
        this.isOwner = globalSettings.authorizedUser.roles.filter(x => x.role == "Owner").length > 0;
        this.isProjectManager = globalSettings.authorizedUser.roles.filter(x => x.role == "Project Manager").length > 0;
    }
}
