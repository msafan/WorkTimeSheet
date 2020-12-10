import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './pages/home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { LoginComponent } from './pages/login/login.component';
import { HeaderComponent } from './components/header/header.component';
import { SideBarComponent } from './components/side-bar/side-bar.component';
import { LogoutComponent } from './pages/logout/logout.component';
import { UsersComponent } from './pages/users/users.component';
import { OrganiztionComponent } from './pages/organiztion/organiztion.component';
import { ReportComponent } from './pages/report/report.component';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ProjectComponent } from './pages/project/project.component';
import { ProjectService } from './services/project.service';
import { GlobalSettings } from './models/global-settings';
import { MomentModule } from 'ngx-moment';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
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
      { path: 'project/:id', component: ProjectComponent },
      { path: 'counter2', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
