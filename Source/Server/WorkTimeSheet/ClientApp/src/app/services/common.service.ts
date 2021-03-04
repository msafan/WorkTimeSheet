import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Observable } from 'rxjs';
import { NgxNotificationStatusMsg } from '../lib/NotificationMsgLibrary/ngx-notification-msg.component';
import { NgxNotificationMsgService } from '../lib/NotificationMsgLibrary/ngx-notification-msg.serice';
import { GlobalSettings } from '../models/global-settings';
import { BaseWebApiService } from './base-web-api.service';

@Injectable({
    providedIn: 'root'
})
export class CommonService extends BaseWebApiService {

    private api = 'api/common/';


    constructor(private ngxNotificationMsgService: NgxNotificationMsgService,
        public modalService: BsModalService, globalSettings: GlobalSettings,
        public httpClient: HttpClient, @Inject('BASE_URL') public baseUrl: string) {
        super(globalSettings, httpClient, baseUrl);
    }

    public toTimeFormat(totalSeconds: number): string {
        const seconds = totalSeconds % 60;
        const totalMinutes = Math.floor(totalSeconds / 60);
        const minutes = totalMinutes % 60;
        const hours = Math.floor(totalMinutes / 60);
        const totalHours = hours % 24;
        const days = Math.floor(totalHours / 24);

        return `${this.setPadding(days)}:${this.setPadding(hours)}:${this.setPadding(minutes)}:${this.setPadding(seconds)}`;
    }

    public toLocalTime(dateTime: string): Date {
        const date = new Date(dateTime);
        return new Date(Date.UTC(date.getFullYear(),
            date.getMonth(), date.getDate(), date.getHours(), date.getMinutes(),
            date.getSeconds(), date.getMilliseconds()));
    }

    public getFirstDateOfMonth(): string {
        var date = new Date();
        return new Date(date.getFullYear(), date.getMonth(), 1).toUTCString();
    }

    public getLastDateOfMonth(): string {
        var date = new Date();
        return new Date(date.getFullYear(),
            date.getMonth(),
            this.daysInMonth(date.getMonth() + 1, date.getFullYear())).toUTCString();
    }

    private daysInMonth(month, year): number {
        return new Date(year, month, 0).getDate();
    }

    private setPadding(number: number): string {
        return number.toString().padStart(2, "0");
    }

    public showError(header: string, message: string) {
        this.ngxNotificationMsgService.open({
            status: NgxNotificationStatusMsg.FAILURE,
            header: header,
            messages: [message]
        });
    }

    public showException(header: string, exception: any) {
        this.ngxNotificationMsgService.open({
            status: NgxNotificationStatusMsg.FAILURE,
            header: header,
            messages: [exception.error.message]
        });
    }

    public ShowInfo(header: string, message: string) {
        this.ngxNotificationMsgService.open({
            status: NgxNotificationStatusMsg.INFO,
            header: header,
            messages: [message]
        });
    }

    public showSuccess(header: string, message: string) {
        this.ngxNotificationMsgService.open({
            status: NgxNotificationStatusMsg.SUCCESS,
            header: header,
            messages: [message]
        });
    }

    public downloadFile(data: any, contentType: any, fileName: any): void {
        let blob = new Blob([data], { type: contentType });
        var blobURL = window.URL.createObjectURL(blob);
        var anchor = document.createElement("a");
        anchor.download = fileName;
        anchor.href = blobURL;
        anchor.click();
    }

    public getApkFile(): Observable<any> {
        return this.httpClient.get(this.api + "apk", { headers: this.getHeader(), responseType: 'arraybuffer' });
    }
}