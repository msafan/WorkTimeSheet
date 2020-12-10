import { Component, OnInit } from '@angular/core';
import * as moment from 'moment';
import { CurrentWork } from 'src/app/models/current-work';
import { GlobalSettings } from 'src/app/models/global-settings';
import { ProjectFilterModel } from 'src/app/models/project-filter-model';
import { ProjectModel } from 'src/app/models/project-model';
import { WorkLogFilterModel } from 'src/app/models/work-log-filter-model';
import { WorkLogModel } from 'src/app/models/work-log-model';
import { ProjectService } from 'src/app/services/project.service';
import { UserService } from 'src/app/services/user.service';
import { WorkLogService } from 'src/app/services/work-log.service';
import { WorkService } from 'src/app/services/work.service';

declare function initializeJS(): void;

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})

export class HomeComponent implements OnInit {

  public projects: ProjectModel[] = [];
  public selectedProject: number;
  public disableProjectSelection: boolean = false;
  public currentWork: CurrentWork;
  public days: number;
  public hours: number;
  public minutes: number;
  public seconds: number;
  public remarks: string;
  public workLogs: WorkLogModel[] = [];
  public totalWorkTime: number = 0;

  constructor(public projectService: ProjectService, public globalSettings: GlobalSettings,
    public userService: UserService, public workService: WorkService, public workLogService: WorkLogService) {

  }

  ngOnInit(): void {
    initializeJS();
    this.getWorkStatus();
  }

  public startWork(): void {
    this.workService.startWork(this.selectedProject).subscribe(result => {
      this.getWorkStatus();
    }, error => {

    });
  }

  public stopWork(): void {
    this.workService.stopWork(this.remarks).subscribe(result => {
      this.getWorkStatus();
    }, error => {

    });
  }

  private getWorkStatus() {
    this.workLogs = [];
    const projectFilterModel: ProjectFilterModel = new ProjectFilterModel();
    projectFilterModel.overridePageSize = true;
    projectFilterModel.pageSize = -1;
    projectFilterModel.userId = this.globalSettings.authorizedUser.userId;

    this.workService.GetCurrentWork().subscribe(currentWork => {
      this.currentWork = currentWork;
      const startDate = new Date(this.currentWork.startDateTime);
      this.currentWork.startDateTime = new Date(Date.UTC(startDate.getFullYear(),
        startDate.getMonth(), startDate.getDate(), startDate.getHours(), startDate.getMinutes(),
        startDate.getSeconds(), startDate.getMilliseconds()));

      this.calculateTime();
      setInterval(() => {
        this.calculateTime();
      }, 1000);


      this.projectService.getAll(projectFilterModel).subscribe(response => {
        this.projects = response.items;
        this.disableProjectSelection = false;
        this.selectedProject = null;
        if (currentWork.projectId != null) {
          this.selectedProject = this.projects.filter(x => x.id == this.currentWork.projectId)[0].id;
          this.disableProjectSelection = true;
        } else if (this.projects.length == 1)
          this.selectedProject = this.projects[0].id;

        if (this.selectedProject != null) {
          const workLogFilterModel = new WorkLogFilterModel();
          workLogFilterModel.noPagination();
          workLogFilterModel.projectId = this.selectedProject;
          workLogFilterModel.userId = this.globalSettings.authorizedUser.userId;
          this.workLogService.getAll(workLogFilterModel).subscribe(response => {
            this.workLogs = response.paginatedResults.items;
            this.totalWorkTime = response.totalTime.totalSeconds;
          }, error => {

          });
        }
      }, error => {

      });
    }, error => {

    });
  }

  private calculateTime(): void {
    const startDateTime = moment(this.currentWork.startDateTime).valueOf();
    const currentDateTime = Date.now();
    const timerDifference = currentDateTime - startDateTime;
    const seconds = Math.floor(timerDifference / 1000);
    this.seconds = seconds % 60;
    const minutes = Math.floor(seconds / 60);
    this.minutes = minutes % 60;
    const hours = Math.floor(minutes / 60);
    this.hours = hours % 24;
    this.days = Math.floor(hours / 24);
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

  public projectChanged(): void {
    this.workLogs = [];
    this.totalWorkTime = 0;

    const workLogFilterModel = new WorkLogFilterModel();
    workLogFilterModel.noPagination();
    workLogFilterModel.userId = this.globalSettings.authorizedUser.userId;
    workLogFilterModel.projectId = this.selectedProject;
    this.workLogService.getAll(workLogFilterModel).subscribe(response => {
      this.workLogs = response.paginatedResults.items;
      this.totalWorkTime = response.totalTime.totalSeconds;
    }, error => {

    });
  }
}
