import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GlobalSettings } from 'src/app/models/global-settings';
import { ProjectModel } from 'src/app/models/project-model';
import { UserModel } from 'src/app/models/user-model';
import { CommonService } from 'src/app/services/common.service';
import { NavigationService } from 'src/app/services/navigation.service';
import { ProjectService } from 'src/app/services/project.service';
import { UserService } from 'src/app/services/user.service';
import { BaseComponent } from '../base-component';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})
export class ProjectComponent extends BaseComponent implements OnInit {
  public project: ProjectModel = new ProjectModel();
  public projectToEdit: ProjectModel = new ProjectModel();
  public members: UserModel[] = [];
  public allUsers: UserModel[] = [];
  public selectedUsers: UserModel[] = [];
  public availableUsers: UserModel[] = [];
  public leftList: number[] = [];
  public rightList: number[] = [];
  constructor(private globalSettings: GlobalSettings, private route: ActivatedRoute,
    private projectService: ProjectService, private navigationService: NavigationService,
    private userService: UserService, commonService: CommonService) {
    super(globalSettings, commonService);
  }

  ngOnInit(): void {
    const projectId = Number(this.route.snapshot.paramMap.get('id'));
    if (projectId == NaN)
      this.navigationService.back();

    this.projectService.getById(projectId).subscribe(project => {
      this.project = project;
      this.projectToEdit = JSON.parse(JSON.stringify(project));

      this.fetchProjectMembers();
    }, error => {
      this.showException('Error', error);
      this.navigationService.back();
    });
  }

  public editProject(): void {
    this.projectService
      .edit(this.projectToEdit.id, this.projectToEdit)
      .subscribe(project => {
        this.project = project;
        this.projectToEdit = JSON.parse(JSON.stringify(project));
      }, error => {
        this.showException('Error', error);
      });
  }

  public initManage(): void {
    this.userService.getAll().subscribe(response => {
      this.allUsers = response.items;
      this.selectedUsers = this.allUsers.filter(x => this.members.filter(y => y.id == x.id).length > 0);
      this.availableUsers = this.allUsers.filter(x => this.members.filter(y => y.id == x.id).length <= 0);
    }, error => {
      this.showException('Error', error);
    });
  }

  public updateMembers(): void {
    this.projectService.updateMembers(this.project.id, this.selectedUsers.map(x => x.id)).subscribe(result => {
      this.fetchProjectMembers();
    }, error => {
      this.showException('Error', error);
    });
  }

  public addUser(): void {
    this.leftList = this.leftList.map(x => Number(x));
    const selectedItems = this.availableUsers.filter(x => this.leftList.indexOf(x.id) >= 0);
    selectedItems.forEach(x => this.selectedUsers.push(x));
    this.availableUsers = this.availableUsers.filter(x => this.leftList.indexOf(x.id) < 0);
    this.leftList = [];
  }

  public removeUser(): void {
    this.rightList = this.rightList.map(x => Number(x)).filter(x => x != this.globalSettings.authorizedUser.userId);
    const selectedItems = this.selectedUsers.filter(x => this.rightList.indexOf(x.id) >= 0);
    selectedItems.forEach(x => this.availableUsers.push(x));
    this.selectedUsers = this.selectedUsers.filter(x => this.rightList.indexOf(x.id) < 0);
    this.rightList = [];
  }

  private fetchProjectMembers(): void {
    this.projectService.getAllMembers(this.project.id).subscribe(response => {
      this.members = response.items;
    }, error => {
      this.showException('Error', error);
      this.navigationService.back();
    });
  }
}
