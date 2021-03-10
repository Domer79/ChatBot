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
import { QuestionsPageComponent } from './questions/questions-page/questions-page.component';
import { EditQuestionDialogComponent } from './questions/edit-question-dialog/edit-question-dialog.component';
import {MatStepperModule} from "@angular/material/stepper";
import {MatRadioModule} from "@angular/material/radio";
import { ConfirmComponent } from './dialogs/confirm/confirm.component';
import { OperatorsComponent } from './security/operators/operators.component';
import { OperatorEditDialogComponent } from './security/operator-edit-dialog/operator-edit-dialog.component';
import {MatCheckboxModule} from "@angular/material/checkbox";
import {ChatDatePipe} from "./pipes/chatdate.pipe";
import { QuestionElementComponent } from './questions/question-element/question-element.component';
import { DialogFilterComponent } from './dialogs/dialog-filter/dialog-filter.component';
import { SelectComponent } from './components/select/select.component';
import { TestComponent } from './test/test.component';
import { DatePickerComponent } from './components/date-picker/date-picker.component';
import {MatDatepickerModule} from "@angular/material/datepicker";
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import { FilterInputComponent } from './components/filter-input/filter-input.component';
import {AuthDirective} from "./directives/auth.directive";
import { AppComponent } from './app/app.component';
import {ScrollControlDirective} from "./directives/scroll-control.directive";
import { ReportsComponent } from './reports/reports.component';

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
    ChatDatePipe,
    QuestionsPageComponent,
    EditQuestionDialogComponent,
    ConfirmComponent,
    OperatorsComponent,
    OperatorEditDialogComponent,
    QuestionElementComponent,
    DialogFilterComponent,
    SelectComponent,
    TestComponent,
    DatePickerComponent,
    FilterInputComponent,
    AuthDirective,
    ScrollControlDirective,
    AppComponent,
    ReportsComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    RouterModule,
    FormsModule,
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
    MatStepperModule,
    MatRadioModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatMomentDateModule,
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
  bootstrap: [AppComponent]
})
export class AppModule { }
