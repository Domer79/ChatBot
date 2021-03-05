import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';

import { MsgContainerComponent } from './msg-container/msg-container.component';
import { ChatBotButtonComponent } from './chat-bot-button/chat-bot-button.component';
import { ChatManagerComponent } from './chat-manager/chat-manager.component';
import { ChatEditorComponent } from './chat-editor/chat-editor.component';
import {FormsModule} from "@angular/forms";
import { MsgContentComponent } from './msg-content/msg-content.component';
import {ClientMsgDispatcher} from './services/client-msg-dispatcher.service';
import { DomChangeDirective } from '../directives/dom-change.directive';
import { ScrollControlDirective } from '../directives/scroll-control.directive';
import { EmojiEditorComponent } from './emoji-editor/emoji-editor.component';
import { ChatHeaderComponent } from './chat-header/chat-header.component';
import { ClientMessageComponent } from './client-message/client-message.component';
import { OperatorMessageComponent } from './operator-message/operator-message.component';
import { ChatDialogComponent } from './dialog/chat-dialog.component';
import { BaseChatHeaderComponent } from './base-chat-header/base-chat-header.component';
import { QuestionsHeaderComponent } from './questions/questions-header/questions-header.component';
import { QuestionsComponent } from './questions/questions/questions.component';
import {QuestionsProviderService} from './services/questions-provider.service';
import { PageDispatcherComponent } from './page-dispatcher/page-dispatcher.component';
import {PageDispatcherService} from './services/page-dispatcher.service';
import { PageHostDirective } from '../directives/page-host.directive';
import {TokenService} from './services/token.service';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {ApiInterceptor} from './services/api-interceptor.service';
import { ChatDatePipe } from './chatdate.pipe';
import { MainQuestionsComponent } from './questions/main-questions/main-questions.component';
import { MainQuestionsHeaderComponent } from './questions/main-questions-header/main-questions-header.component';
import { FirstQuestionsComponent } from './questions/first-questions/first-questions.component';
import { BackService } from '../abstracts/BackService';
import { SearchQuestionsComponent } from './questions/search-questions/search-questions.component';
import { AuthFormComponent } from './auth-form/auth-form.component';
import { AuthFormHederComponent } from './auth-form-heder/auth-form-heder.component';
import {IMaskModule} from 'angular-imask';
import {CommonService} from './services/common.service';

@NgModule({
  declarations: [
    MsgContainerComponent,
    ChatBotButtonComponent,
    ChatManagerComponent,
    ChatEditorComponent,
    MsgContentComponent,
    DomChangeDirective,
    ScrollControlDirective,
    EmojiEditorComponent,
    ChatHeaderComponent,
    ClientMessageComponent,
    OperatorMessageComponent,
    ChatDialogComponent,
    BaseChatHeaderComponent,
    QuestionsHeaderComponent,
    QuestionsComponent,
    PageDispatcherComponent,
    PageHostDirective,
    ChatDatePipe,
    MainQuestionsComponent,
    MainQuestionsHeaderComponent,
    FirstQuestionsComponent,
    SearchQuestionsComponent,
    AuthFormComponent,
    AuthFormHederComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    IMaskModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ApiInterceptor, multi: true },
    ClientMsgDispatcher,
    QuestionsProviderService,
    PageDispatcherService,
    TokenService,
    CommonService
  ],
  bootstrap: [ChatManagerComponent]
})
export class AppModule { }
