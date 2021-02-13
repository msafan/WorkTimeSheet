import { TemplateRef } from "@angular/core";
import { BsModalRef } from "ngx-bootstrap/modal";
import { GlobalSettings } from "../models/global-settings";
import { CommonService } from "../services/common.service";

export class BaseComponent {
    public isOwner: boolean = false;
    public isProjectManager: boolean = false;
    private modalRef: BsModalRef;

    constructor(globalSettings: GlobalSettings, protected commonService: CommonService) {
        try {
            this.isOwner = globalSettings.authorizedUser.roles.filter(x => x.role == "Owner").length > 0;
        } catch {
            this.isOwner = false;
        }

        try {
            this.isProjectManager = globalSettings.authorizedUser.roles.filter(x => x.role == "Project Manager").length > 0;
        } catch {
            this.isProjectManager = false;
        }
    }

    public openModal(template: TemplateRef<any>) {
        this.modalRef = this.commonService.modalService.show(template);
    }

    public closeModal() {
        this.modalRef.hide();
    }

    public showError(header: string, message: string) {
        this.commonService.showError(header, message);
    }

    public showException(header: string, exception: any) {
        this.commonService.showException(header, exception);
    }

    public ShowInfo(header: string, message: string) {
        this.commonService.ShowInfo(header, message);
    }

    public showSuccess(header: string, message: string) {
        this.commonService.showSuccess(header, message);
    }
}
