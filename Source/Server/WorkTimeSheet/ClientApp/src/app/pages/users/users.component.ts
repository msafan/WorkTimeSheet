import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CreateUserModel } from 'src/app/models/create-user-model';
import { GlobalSettings } from 'src/app/models/global-settings';
import { UserFilterModel } from 'src/app/models/user-filter-model';
import { UserModel } from 'src/app/models/user-model';
import { UserRole } from 'src/app/models/user-role';
import { UserRoleSelection } from 'src/app/models/user-role-selection';
import { UserService } from 'src/app/services/user.service';
import { BaseComponent } from '../base-component';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent extends BaseComponent implements OnInit {
  public users: UserModel[] = [];
  public userToCreate: CreateUserModel = new CreateUserModel();
  public filterModel: UserFilterModel = new UserFilterModel();
  public userRoles: UserRole[] = [];
  public userRolesSelection: UserRoleSelection[] = [];
  public userToEdit: UserModel = new UserModel();

  constructor(globalSettings: GlobalSettings, private userService: UserService, private router: Router) {
    super(globalSettings);
  }

  ngOnInit() {
    if (!this.isOwner)
      this.router.navigate(['/dashboard']);

    this.fetchUsers();
    this.userService.getAllUserRoles().subscribe(roles => {
      this.userRoles = roles;
      this.mapUserRoles();
    }, error => {

    });
  }

  private mapUserRoles(): void {
    this.userRolesSelection = this.userRoles.map(x => {
      const userRoleSelection: UserRoleSelection = new UserRoleSelection();
      userRoleSelection.id = x.id;
      userRoleSelection.role = x.role
      return userRoleSelection;
    })
  }

  private fetchUsers(): void {
    this.userService
      .getAll(this.filterModel)
      .subscribe(response => {
        this.users = response.items;
      }, error => {

      });
  }

  public addUser(): void {
    const roleIds = this.userRolesSelection.filter(x => x.isSelected).map(x => x.id)
    if (roleIds.length <= 0)
      return;

    if (this.userToCreate.password != this.userToCreate.confirmPassword)
      return;

    const createUserModel: CreateUserModel = new CreateUserModel();
    createUserModel.name = this.userToCreate.name;
    createUserModel.email = this.userToCreate.email;
    createUserModel.password = this.userToCreate.password;
    createUserModel.roleIds = roleIds;
    this.userService.createUser(createUserModel).subscribe(response => {
      this.cleanUp();
    }, error => { });
  }

  public initEditUser(userId: number): void {
    this.userService.getById(userId).subscribe(response => {
      this.userToEdit = response;
      this.mapUserRoles();
      response.userRoles.forEach(x => {
        this.userRolesSelection
          .filter(y => y.id == x.id)
          .forEach(y => y.isSelected = true);
      });
    }, error => { });
  }

  public editUser(): void {
    const roleIds = this.userRolesSelection.filter(x => x.isSelected).map(x => x.id)
    if (roleIds.length <= 0)
      return;

    this.userToEdit.userRoles = this.userRolesSelection.filter(x => x.isSelected).map(x => {
      const userRole: UserRole = new UserRole();
      userRole.id = x.id;
      userRole.role = x.role;
      return userRole;
    });

    this.userService.edituser(this.userToEdit.id, this.userToEdit).subscribe(response => {
      this.cleanUp();
    }, error => { });
  }

  public deleteUser(userId: number): void {
    this.userService.deleteUser(userId).subscribe(result => {
      this.cleanUp();
    }, error => {

    });
  }

  private cleanUp(): void {
    this.fetchUsers();
    this.mapUserRoles();
    this.userToCreate = new CreateUserModel();
    this.userToEdit = new UserModel();
  }
}
