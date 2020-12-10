import { Component, OnInit } from '@angular/core';
import { GlobalSettings } from 'src/app/models/global-settings';
import { WorkLogFilterModel } from 'src/app/models/work-log-filter-model';
import { WorkLogModel } from 'src/app/models/work-log-model';
import { WorkLogService } from 'src/app/services/work-log.service';
import { BaseComponent } from '../base-component';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent extends BaseComponent implements OnInit {
  public workLogs: WorkLogModel[];
  public totalWorkTime: number = 0;
  public filterModel: WorkLogFilterModel = new WorkLogFilterModel();

  constructor(public workLogService: WorkLogService, public globalSettings: GlobalSettings) {
    super(globalSettings);
  }

  ngOnInit() {
    this.filterModel.noPagination();
    if (!this.isOwner && !this.isProjectManager)
      this.filterModel.userId = this.globalSettings.authorizedUser.userId;
      
    this.filterReport();
  }

  public toTimeFormat(totalSeconds: number): string {
    const seconds = totalSeconds % 60;
    const totalMinutes = Math.floor(totalSeconds / 60);
    const minutes = totalMinutes % 60;
    const hours = Math.floor(totalMinutes / 60);
    const totalHours = hours % 24;
    const days = Math.floor(totalHours / 24);

    return `${days}:${hours}:${minutes}:${seconds}`;
  }

  public toLocalTime(dateTime: string): Date {
    const date = new Date(dateTime);
    return new Date(Date.UTC(date.getFullYear(),
      date.getMonth(), date.getDate(), date.getHours(), date.getMinutes(),
      date.getSeconds(), date.getMilliseconds()));
  }

  public filterReport(): void {
    this.workLogService.getAll(this.filterModel).subscribe(response => {
      this.workLogs = response.paginatedResults.items;
      this.totalWorkTime = response.totalTime.totalSeconds;
    }, error => {

    });
  }
}
