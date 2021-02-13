import { Component, OnInit } from '@angular/core';
import * as moment from 'moment';
import { CurrentWork } from 'src/app/models/current-work';
import { GlobalSettings } from 'src/app/models/global-settings';
import { ProjectFilterModel } from 'src/app/models/project-filter-model';
import { ProjectModel } from 'src/app/models/project-model';
import { WorkLogFilterModel } from 'src/app/models/work-log-filter-model';
import { WorkLogModel } from 'src/app/models/work-log-model';
import { CommonService } from 'src/app/services/common.service';
import { ProjectService } from 'src/app/services/project.service';
import { UserService } from 'src/app/services/user.service';
import { WorkLogService } from 'src/app/services/work-log.service';
import { WorkService } from 'src/app/services/work.service';
import { BaseComponent } from '../base-component';

declare function initializeJS(): void;

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})

export class HomeComponent extends BaseComponent implements OnInit {

  public projects: ProjectModel[] = [];
  public selectedProject: number;
  public disableProjectSelection: boolean = false;
  public currentWork: CurrentWork;
  public timer: string;
  public remarks: string;
  public workLogs: WorkLogModel[] = [];
  public totalWorkTime: number = 0;

  constructor(private projectService: ProjectService, private globalSettings: GlobalSettings,
    private userService: UserService, private workService: WorkService, private workLogService: WorkLogService,
    commonService: CommonService) {
    super(globalSettings, commonService);

  }

  ngOnInit(): void {
    initializeJS();
    this.getWorkStatus();
  }

  public startWork(): void {
    this.workService.startWork(this.selectedProject).subscribe(result => {
      this.showSuccess('Work Started', 'Work started on ' + this.projects.filter(x => x.id == this.selectedProject)[0].name);
      this.getWorkStatus();
    }, error => {
      this.showError('Cannot start work', 'Work might have already been started in different instance');
      this.getWorkStatus();
    });
  }

  public stopWork(): void {
    if (this.remarks == undefined || this.remarks == "") {
      this.showError('Remarks missing', 'Need remarks for work.');
      return;
    }
    this.workService.stopWork(this.remarks).subscribe(result => {
      this.showSuccess('Work stopped', 'Work stopped on ' + this.projects.filter(x => x.id == this.selectedProject)[0].name);
      this.getWorkStatus();
    }, error => {
      this.showError('Cannot stop work', 'Work might have already been stopped in different instance')
      this.getWorkStatus();
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

        if (this.selectedProject != null)
          this.getWorkLogForThisMonth();
      }, error => {
        this.showException('Error', error);
      });
    }, error => {
      this.showException('Error', error);
    });
  }

  private calculateTime(): void {
    const startDateTime = moment(this.currentWork.startDateTime).valueOf();
    const currentDateTime = Date.now();
    const timerDifference = currentDateTime - startDateTime;
    const seconds = Math.floor(timerDifference / 1000);
    this.timer = this.commonService.toTimeFormat(seconds);
  }

  public projectChanged(): void {
    this.workLogs = [];
    this.totalWorkTime = 0;

    this.getWorkLogForThisMonth();
  }

  private getWorkLogForThisMonth() {
    const workLogFilterModel = new WorkLogFilterModel();
    workLogFilterModel.noPagination();
    workLogFilterModel.projectIds = [this.selectedProject];
    workLogFilterModel.userIds = [this.globalSettings.authorizedUser.userId];
    workLogFilterModel.startDate = this.commonService.getFirstDateOfMonth();
    workLogFilterModel.endDate = this.commonService.getLastDateOfMonth();
    this.workLogService.getAll(workLogFilterModel).subscribe(response => {
      this.workLogs = response.paginatedResults.items;
      this.totalWorkTime = response.totalTime;
    }, error => {
      this.showException('Error', error);
    });
  }
}
