import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {HomeComponent} from "./home/home.component";
import {CreateTicketComponent} from "./create-ticket/create-ticket.component";
import {StatsComponent} from "./stats/stats.component";

const routes: Routes = [
  {path: "", component: HomeComponent},
  {path: "create-ticket", component: CreateTicketComponent},
  {path: "stats", component: StatsComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
