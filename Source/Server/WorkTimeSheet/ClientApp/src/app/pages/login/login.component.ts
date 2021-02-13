import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CreateUserModel } from 'src/app/models/create-user-model';
import { GlobalSettings } from 'src/app/models/global-settings';
import { RegistrationModel } from 'src/app/models/registration-model';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { CommonService } from 'src/app/services/common.service';
import { OrganizationService } from 'src/app/services/organization.service';
import { BaseComponent } from '../base-component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent extends BaseComponent implements OnInit {

  public isRegisterPage: boolean = false;
  public loginForm: FormGroup;
  public registerForm: FormGroup;

  constructor(private globalSettings: GlobalSettings,
    private router: Router,
    private authenticationService: AuthenticationService,
    private formBuilder: FormBuilder,
    private origanizationService: OrganizationService,
    commonService: CommonService) {
    super(globalSettings, commonService);
  }

  ngOnInit() {
    this.resetLoginForm();
    this.resetRegisterForm();
  }

  public login(): void {
    if (this.loginForm.invalid)
      return;

    this.authenticationService
      .login(this.loginForm.controls.email.value, this.loginForm.controls.password.value)
      .subscribe(result => {
        this.resetLoginForm();
        this.globalSettings.isLoggedIn = true;
        localStorage.setItem('authorizedUser', JSON.stringify(result));
        this.globalSettings.authorizedUser = result;
        this.router.navigate(['/dashboard']);
      }, error => {
        this.showException('Login Failed', error);
      });
  }

  public register(): void {
    if (this.registerForm.invalid)
      return;
    if (this.registerForm.controls.password.value != this.registerForm.controls.confirmPassword.value)
      return;

    const registrationModel = new RegistrationModel();
    registrationModel.name = this.registerForm.controls.organizationName.value;
    registrationModel.user = new CreateUserModel();
    registrationModel.user.name = this.registerForm.controls.name.value;
    registrationModel.user.email = this.registerForm.controls.email.value;
    registrationModel.user.password = this.registerForm.controls.password.value;

    this.origanizationService
      .register(registrationModel)
      .subscribe(response => {
        this.isRegisterPage = false;
        this.resetRegisterForm();
        this.showSuccess('Registration completed', 'Successfully registered user and origanization');
      }, error => {
        this.showException('Error', error);
      });
  }

  public changeToRegistration(): void {
    this.isRegisterPage = true;
  }

  public backToLogin(): void {
    this.isRegisterPage = false;
  }

  private resetLoginForm(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', Validators.required, Validators.email],
      password: ['', Validators.required]
    });
  }

  private resetRegisterForm(): void {
    this.registerForm = this.formBuilder.group({
      name: ['', Validators.required],
      email: ['', Validators.required, Validators.email],
      organizationName: ['', Validators.required],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required],
    });
  }

}
