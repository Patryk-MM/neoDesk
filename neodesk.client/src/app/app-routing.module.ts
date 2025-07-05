import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from "./home/home.component";
import { CreateTicketComponent } from "./create-ticket/create-ticket.component";
import { StatsComponent } from "./stats/stats.component";
import { TicketDetailComponent } from "./ticket-detail/ticket-detail.component";
import { LoginComponent } from "./login/login.component";
import { UserManagementComponent } from "./user-management/user-management.component";
import { AuthGuard } from "./guards/auth.guard";
import { NoAuthGuard } from "./guards/no-auth.guard";

const routes: Routes = [
  {
    path: "login",
    component: LoginComponent,
    canActivate: [NoAuthGuard]
  },
  {
    path: "",
    component: HomeComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "create-ticket",
    component: CreateTicketComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "ticket/:id",
    component: TicketDetailComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "stats",
    component: StatsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "user-management",
    component: UserManagementComponent,
    canActivate: [AuthGuard],
    data: { roles: ['Admin'] } // Only admins can access
  },
  {
    path: "**",
    redirectTo: ""
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
