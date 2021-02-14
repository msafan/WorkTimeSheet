import { ClipboardModule } from '@angular/cdk/clipboard';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { AngularMultiSelectModule } from 'angular2-multiselect-dropdown';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TimepickerModule } from 'ngx-bootstrap/timepicker';
import { MomentModule } from 'ngx-moment';
import { AppComponent } from './app.component';
import { HeaderComponent } from './components/header/header.component';
import { SideBarComponent } from './components/side-bar/side-bar.component';
import { NgxNotificationMsgModule } from './lib/NotificationMsgLibrary/ngx-notification-msg.module';
import { AccessTokensComponent } from './pages/access-tokens/access-tokens.component';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { LogoutComponent } from './pages/logout/logout.component';
import { OrganiztionComponent } from './pages/organiztion/organiztion.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { ProjectComponent } from './pages/project/project.component';
import { ReportComponent } from './pages/report/report.component';
import { UsersComponent } from './pages/users/users.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    HeaderComponent,
    SideBarComponent,
    LogoutComponent,
    UsersComponent,
    OrganiztionComponent,
    ReportComponent,
    ProjectComponent,
    ProfileComponent,
    AccessTokensComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    MomentModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: LoginComponent, pathMatch: 'full' },
      { path: 'dashboard', component: HomeComponent },
      { path: 'logout', component: LogoutComponent },
      { path: 'users', component: UsersComponent, },
      { path: 'organization', component: OrganiztionComponent },
      { path: 'report', component: ReportComponent },
      { path: 'profile', component: ProfileComponent },
      { path: 'accessTokens', component: AccessTokensComponent },
      { path: 'project/:id', component: ProjectComponent }
    ]),
    BrowserAnimationsModule,
    ClipboardModule,
    NgxNotificationMsgModule,
    MatSelectModule,
    AngularMultiSelectModule,
    ModalModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TimepickerModule.forRoot(),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
