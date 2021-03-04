import { Component, OnInit, TemplateRef } from '@angular/core';
import { GlobalSettings } from 'src/app/models/global-settings';
import { Pagination } from 'src/app/models/pagination';
import { ProjectFilterModel } from 'src/app/models/project-filter-model';
import { ProjectModel } from 'src/app/models/project-model';
import { UserFilterModel } from 'src/app/models/user-filter-model';
import { UserModel } from 'src/app/models/user-model';
import { WorkLogFilterModel } from 'src/app/models/work-log-filter-model';
import { WorkLogModel } from 'src/app/models/work-log-model';
import { CommonService } from 'src/app/services/common.service';
import { ProjectService } from 'src/app/services/project.service';
import { UserService } from 'src/app/services/user.service';
import { WorkLogService } from 'src/app/services/work-log.service';
import { BaseComponent } from '../base-component';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent extends BaseComponent implements OnInit {
  public workLogs: WorkLogModel[];
  public projects: ProjectModel[];
  public users: UserModel[];
  public totalWorkTime: number = 0;
  public filterModel: WorkLogFilterModel = new WorkLogFilterModel();
  public selectedProjects: ProjectModel[];
  public selectedUsers: UserModel[];
  public dateRange: Date[];

  constructor(private workLogService: WorkLogService, private globalSettings: GlobalSettings,
    private projectService: ProjectService, private userService: UserService,
    public commonService: CommonService) {
    super(globalSettings, commonService);

    this.dateRange = [
      new Date(this.commonService.getFirstDateOfMonth()),
      new Date(this.commonService.getLastDateOfMonth())
    ];
  }

  dropdownSettings = {};

  ngOnInit() {
    this.dropdownSettings = {
      singleSelection: false,
      text: "",
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      enableSearchFilter: true,
      classes: "myclass custom-class",
      labelKey: "name"
    };

    this.filterModel.noPagination();
    if (!this.isOwner && !this.isProjectManager)
      this.filterModel.userIds = [this.globalSettings.authorizedUser.userId];

    if (this.isOwner) {
      const filterModel = new ProjectFilterModel();
      filterModel.noPagination();
      this.projectService.getAll(filterModel).subscribe(x => {
        this.projects = x.items;
      });
    } else {
      this.projectService.getMyProjects(Pagination.noPagination()).subscribe(x => {
        this.projects = x.items;
      });
    }

    if (this.isOwner || this.isProjectManager) {
      const filterModel = new UserFilterModel();
      filterModel.noPagination();
      this.userService.getAll(filterModel).subscribe(x => {
        this.users = x.items;
      });
    }

    this.filterReport();
  }

  public filterReport(): void {
    if (this.selectedProjects != undefined)
      this.filterModel.projectIds = this.selectedProjects.map(x => x.id);
    if (this.selectedUsers != undefined)
      this.filterModel.userIds = this.selectedUsers.map(x => x.id);

    this.filterModel.startDate = this.dateRange[0].toUTCString();
    if (this.dateRange[1].getHours() == 0 && this.dateRange[1].getMinutes() == 0 && this.dateRange[1].getSeconds() == 0)
      this.filterModel.endDate = new Date(this.dateRange[1].getFullYear(), this.dateRange[1].getMonth(), this.dateRange[1].getDate(),
        23, 59, 59).toUTCString();
    else
      this.filterModel.endDate = this.dateRange[1].toUTCString();

    this.workLogService.getAll(this.filterModel).subscribe(response => {
      this.workLogs = response.paginatedResults.items;
      this.totalWorkTime = response.totalTime;
    }, error => {

    });
  }

  public editWork(id: number, template: TemplateRef<any>): void {
    this.openModal(template);
  }
}
