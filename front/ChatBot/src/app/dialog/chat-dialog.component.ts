import {Component, EventEmitter, OnDestroy, OnInit} from '@angular/core';
import {ClientMsgDispatcher} from '../services/client-msg-dispatcher.service';
import Message from '../../abstracts/message';
import {Observable, Subscription} from 'rxjs';
import {PageDispatcherService} from '../services/page-dispatcher.service';
import {QuestionsComponent} from '../questions/questions/questions.component';
import {map, tap} from 'rxjs/operators';
import {MessageType} from '../../misc/message-type';
import {MainQuestionsComponent} from '../questions/main-questions/main-questions.component';
import {ShowChatEditor} from '../../abstracts/ShowChatEditor';
import {AuthService} from '../services/auth.service';

@Component({
  selector: 'chat-dialog',
  templateUrl: './chat-dialog.component.html',
  styleUrls: ['./dialog.component.sass']
})
export class ChatDialogComponent implements OnInit, OnDestroy, ShowChatEditor {
  private metaSubscription: Subscription;
  private messageSubscription: Subscription;
  messages: Message[] = [];

  constructor(
    private clientMsgDispatcher: ClientMsgDispatcher,
    private pageDispatcherService: PageDispatcherService,
    private authService: AuthService
  ) {
  }

  ngOnInit(): void {
    this.clientMsgDispatcher.loadMessages();
    this.messageSubscription = this.clientMsgDispatcher.messages.pipe(
      map(_ => {
        if (_.type === MessageType.Question){
          const question = JSON.parse(_.content);
          _.questionId = question.Id;
          _.question = question.Question;
          _.content = question.Question;
        }
        else{
          _.content = _.content + ' ' + _.messageDialogId.substring(1, 8);
        }

        return _;
    })).subscribe(msg => {
      debugger;
      this.messages.push(msg);
    });
    this.metaSubscription = this.clientMsgDispatcher.meta.subscribe((msg) => {
      const messages = this.messages.filter(_ => _.id === msg.id);
      if (messages.length > 0) {
        messages[0].status = msg.status;
      }
    });
  }

  ngOnDestroy(): void {
    this.metaSubscription.unsubscribe();
    this.messageSubscription.unsubscribe();
  }

  onOpenQuestions(): void {
    this.pageDispatcherService.showPage(MainQuestionsComponent);
  }

  canShowEditor(): Observable<boolean> {
    return this.authService.isActive();
  }
}
