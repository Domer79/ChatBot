import {Component, OnDestroy, OnInit} from '@angular/core';
import {ClientMsgDispatcher} from '../services/client-msg-dispatcher.service';
import Message from '../../abstracts/message';
import {Subscription} from 'rxjs';

@Component({
  selector: 'chat-dialog',
  templateUrl: './chat-dialog.component.html',
  styleUrls: ['./dialog.component.sass']
})
export class ChatDialogComponent implements OnInit, OnDestroy {
  metaSubscription: Subscription;
  messages: Message[] = [];

  constructor(private clientMsgDispatcher: ClientMsgDispatcher) { }

  ngOnInit(): void {
    this.clientMsgDispatcher.receive(msg => {
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
    this.clientMsgDispatcher.stop();
  }

}
