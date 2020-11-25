import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';

import { MsgContainerComponent } from './msg-container/msg-container.component';
import { ChatBotButtonComponent } from './chat-bot-button/chat-bot-button.component';
import { ChatManagerComponent } from './chat-manager/chat-manager.component';
import { ChatEditorComponent } from './chat-editor/chat-editor.component';
import {FormsModule} from "@angular/forms";
import { MsgContentComponent } from './msg-content/msg-content.component';
import {ClientMsgDispatcher} from '../services/client-msg-dispatcher.service';
import { DomChangeDirective } from '../directives/dom-change.directive';
import { ScrollControlDirective } from '../directives/scroll-control.directive';

@NgModule({
  declarations: [
    MsgContainerComponent,
    ChatBotButtonComponent,
    ChatManagerComponent,
    ChatEditorComponent,
    MsgContentComponent,
    DomChangeDirective,
    ScrollControlDirective,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
  ],
  providers: [
    ClientMsgDispatcher
  ],
  bootstrap: [ChatManagerComponent]
})
export class AppModule { }
