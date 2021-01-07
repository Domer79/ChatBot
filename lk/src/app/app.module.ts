import { BrowserModule } from '@angular/platform-browser';
import {LOCALE_ID, NgModule} from '@angular/core';

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
import {AuthGuard} from "./services/auth-guard.service";
import {RouterModule} from "@angular/router";
import {ApiInterceptor} from "./services/api-interceptor.service";
import {AuthService} from "./services/auth.service";
import {CacheService} from "./services/cache.service";
import { AccessDeniedComponent } from './access-denied/access-denied.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatInputModule} from "@angular/material/input";
import {MatSnackBarModule} from "@angular/material/snack-bar";
import {TokenService} from "./services/token.service";
import { DialogsComponent } from './dialogs/dialogs.component';
import {DialogService} from "./services/dialog.service";
import {MatCardModule} from "@angular/material/card";
import { ClientChatDialogComponent } from './client-chat-dialog/client-chat-dialog.component';
import {MatDialogModule} from "@angular/material/dialog";
import { OperatorMessageComponent } from './dialog/operator-message/operator-message.component';
import { ClientMessageComponent } from './dialog/client-message/client-message.component';
import {MessageService} from "./services/message.service";
import { registerLocaleData } from "@angular/common";
import localeRu from '@angular/common/locales/ru';
import {MatPaginatorModule} from "@angular/material/paginator";
import { DialogStatusPipe } from './pipes/dialog-status.pipe';
import { FioPipe } from './pipes/fio.pipe';

registerLocaleData(localeRu, 'ru');

@NgModule({
  declarations: [
    MainComponent,
    LoginComponent,
    AccessDeniedComponent,
    DialogsComponent,
    ClientChatDialogComponent,
    OperatorMessageComponent,
    ClientMessageComponent,
    DialogStatusPipe,
    FioPipe
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
    MatCardModule,
    MatDialogModule,
    MatPaginatorModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ApiInterceptor, multi: true },
    { provide: LOCALE_ID, useValue: 'ru' },
      AuthService,
      AuthGuard,
      CacheService,
      TokenService,
      DialogService,
      MessageService,
  ],
  bootstrap: [MainComponent]
})
export class AppModule { }
