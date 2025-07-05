import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from "@angular/forms";

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { HomeComponent } from './home/home.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { LucideAngularModule, Headset, Info, Plus, ChartBar, BookOpen, BookOpenText, BookPlus, Settings, Wrench, UserRound } from 'lucide-angular';
import { CreateTicketComponent } from './create-ticket/create-ticket.component'
import { TicketDetailComponent } from './ticket-detail/ticket-detail.component';
import { LoginComponent } from './login/login.component';
import { UserManagementComponent } from './user-management/user-management.component';
import { QuillModule } from "ngx-quill";
import { StatsComponent } from './stats/stats.component';

// Auth
import { AuthInterceptor } from './interceptors/auth.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    HomeComponent,
    SidebarComponent,
    CreateTicketComponent,
    TicketDetailComponent,
    LoginComponent,
    UserManagementComponent,
    StatsComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    LucideAngularModule.pick({
      Headset,
      Info,
      Plus,
      ChartBar,
      BookOpen,
      BookOpenText,
      BookPlus,
      Settings,
      Wrench,
      UserRound
    }),
    QuillModule.forRoot(),
    FormsModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
