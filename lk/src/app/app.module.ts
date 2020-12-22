import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { MainComponent } from './main/main.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatSidenavModule} from "@angular/material/sidenav";
import {MatToolbarModule} from "@angular/material/toolbar";
import {MatIconModule} from "@angular/material/icon";
import {MatButtonModule} from "@angular/material/button";
import {MatTabsModule} from "@angular/material/tabs";
import {MatListModule} from "@angular/material/list";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {MatMenuModule} from "@angular/material/menu";
import { LoginComponent } from './login/login.component';
import {AuthGuard} from "./auth-guard.service";
import {RouterModule} from "@angular/router";
import {ApiInterceptor} from "./api-interceptor.service";
import {AuthService} from "./auth.service";
import {CacheService} from "./cache.service";
import { AccessDeniedComponent } from './access-denied/access-denied.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatInputModule} from "@angular/material/input";
import {MatSnackBarModule} from "@angular/material/snack-bar";
import {TokenService} from "./token.service";
import { DialogsComponent } from './dialogs/dialogs.component';

@NgModule({
  declarations: [
    MainComponent,
    LoginComponent,
    AccessDeniedComponent,
    DialogsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    RouterModule,
    MatSidenavModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatTabsModule,
    MatListModule,
    MatMenuModule,
    MatInputModule,
    ReactiveFormsModule,
    MatSnackBarModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ApiInterceptor, multi: true },
      AuthService,
      AuthGuard,
      CacheService,
      TokenService,
  ],
  bootstrap: [MainComponent]
})
export class AppModule { }
