import { Component, OnInit } from '@angular/core';
import { GlobalSettings } from 'src/app/models/global-settings';
import { OrganizationModel } from 'src/app/models/organization-model';
import { ProjectFilterModel } from 'src/app/models/project-filter-model';
import { ProjectModel } from 'src/app/models/project-model';
import { CommonService } from 'src/app/services/common.service';
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
    private projectService: ProjectService, commonService: CommonService) {
    super(globalSettings, commonService);
  }

  ngOnInit() {
    this.origanizationService
      .getMyOrganization()
      .subscribe(organization => {
        this.organizationModel = organization;
        this.organizationModelToEdit = JSON.parse(JSON.stringify(organization));
      }, error => {
        this.showException('Error', error);
      });

    this.fetchProjects();
  }

  public editOrganization(): void {
    this.origanizationService
      .edit(this.organizationModel.id, this.organizationModelToEdit)
      .subscribe(organization => {
        this.closeModal();
        this.showSuccess('Organization Modified', organization.name);

        this.organizationModel = organization;
        this.organizationModelToEdit = JSON.parse(JSON.stringify(organization));
      }, error => {
        this.showException('Error', error);
      });
  }

  public createProject(): void {
    if (this.projectModel.name == undefined || this.projectModel.name == "") {
      this.showError('Error', 'Please enter project name')
      return;
    }

    if (this.projectModel.description == undefined || this.projectModel.description == "") {
      this.showError('Error', 'Please enter project description')
      return;
    }

    this.projectService.create(this.projectModel).subscribe(response => {
      this.showSuccess('Project Created', response.name)

      this.closeModal();
      this.fetchProjects();
    }, error => {
      this.showException('Error', error);
    });
  }

  private fetchProjects(): void {
    const projectFilterModel: ProjectFilterModel = new ProjectFilterModel();
    projectFilterModel.noPagination();
    this.projectService.getAll(projectFilterModel).subscribe(response => {
      this.projects = response.items;
    }, error => {
      this.showException('Error', error);
    });
  }
}
