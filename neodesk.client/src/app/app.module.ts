import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { HomeComponent } from './home/home.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { LucideAngularModule, Headset, Info, Plus, ChartBar, BookOpen, BookOpenText, BookPlus, Settings, Wrench, UserRound } from 'lucide-angular';
import { CreateTicketComponent } from './create-ticket/create-ticket.component'

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    HomeComponent,
    SidebarComponent,
    CreateTicketComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule,
    LucideAngularModule.pick({Headset, Info, Plus, ChartBar, BookOpen, BookOpenText, BookPlus, Settings, Wrench, UserRound})
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
