import {Component, EventEmitter, OnDestroy, OnInit} from '@angular/core';
import {ClientMsgDispatcher} from '../services/client-msg-dispatcher.service';
import Message from '../../abstracts/message';
import {Subscription} from 'rxjs';
import {MessageService} from '../services/message.service';
import CloseChat from '../../abstracts/CloseChat';
import {PageDispatcherService} from '../services/page-dispatcher.service';
import {QuestionsComponent} from '../questions/questions/questions.component';

@Component({
  selector: 'chat-dialog',
  templateUrl: './chat-dialog.component.html',
  styleUrls: ['./dialog.component.sass']
})
export class ChatDialogComponent implements OnInit, OnDestroy, CloseChat {
  private metaSubscription: Subscription;
  private messageSubscription: Subscription;
  private closedEmitter = new EventEmitter<void>();
  messages: Message[] = [];

  constructor(
    private clientMsgDispatcher: ClientMsgDispatcher,
    private pageDispatcherService: PageDispatcherService
  ) {
  }

  ngOnInit(): void {
    this.clientMsgDispatcher.loadMessages();
    this.messageSubscription = this.clientMsgDispatcher.messages.subscribe(msg => {
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

  passClosedEmitter(closed: EventEmitter<void>): void {
    this.closedEmitter = closed;
  }

  onCloseChat(): void {
    this.closedEmitter.emit();
  }

  onOpenQuestions(): void {
    this.pageDispatcherService.showPage(QuestionsComponent);
  }
}
