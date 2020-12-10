import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { MomentModule } from 'ngx-moment';
import { AppComponent } from './app.component';
import { HeaderComponent } from './components/header/header.component';
import { SideBarComponent } from './components/side-bar/side-bar.component';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { LogoutComponent } from './pages/logout/logout.component';
import { OrganiztionComponent } from './pages/organiztion/organiztion.component';
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
      { path: 'users', component: UsersComponent,  },
      { path: 'organization', component: OrganiztionComponent },
      { path: 'report', component: ReportComponent },
      { path: 'project/:id', component: ProjectComponent }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
