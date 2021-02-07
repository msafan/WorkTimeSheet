import { Component, OnInit } from '@angular/core';
import { GlobalSettings } from 'src/app/models/global-settings';
import { OrganizationModel } from 'src/app/models/organization-model';
import { Pagination } from 'src/app/models/pagination';
import { ProjectFilterModel } from 'src/app/models/project-filter-model';
import { ProjectModel } from 'src/app/models/project-model';
import { OrganizationService } from 'src/app/services/organization.service';
import { ProjectService } from 'src/app/services/project.service';
import { BaseComponent } from '../base-component';

@Component({
  selector: 'app-organiztion',
  templateUrl: './organiztion.component.html',
  styleUrls: ['./organiztion.component.css']
})
export class OrganiztionComponent extends BaseComponent implements OnInit {
  public organizationModel: OrganizationModel = new OrganizationModel();
  public projectModel: ProjectModel = new ProjectModel();
  public organizationModelToEdit: OrganizationModel = new OrganizationModel();
  public projects: ProjectModel[] = [];

  constructor(private origanizationService: OrganizationService, private globalSettings: GlobalSettings, 
    private projectService: ProjectService) {
    super(globalSettings);
  }

  ngOnInit() {
    this.origanizationService
      .getByUserId(this.globalSettings.authorizedUser.userId)
      .subscribe(organization => {
        this.organizationModel = organization;
        this.organizationModelToEdit = JSON.parse(JSON.stringify(organization));
      }, error => {

      });

    this.fetchProjects();
  }

  public editOrganization(): void {
    this.origanizationService
      .edit(this.organizationModel.id, this.organizationModelToEdit)
      .subscribe(organization => {
        this.organizationModel = organization;
        this.organizationModelToEdit = JSON.parse(JSON.stringify(organization));
      }, error => {

      });
  }

  public createProject(): void {
    this.projectService.create(this.projectModel).subscribe(response => {
      this.fetchProjects();
    }, error => {

    });
  }

  private fetchProjects(): void {
    const pagination = Pagination.noPagination();
    const projectFilterModel: ProjectFilterModel = new ProjectFilterModel();
    projectFilterModel.noPagination();
    this.projectService.getAll(projectFilterModel).subscribe(response => {
      this.projects = response.items;
    }, error => {

    });
  }
}
