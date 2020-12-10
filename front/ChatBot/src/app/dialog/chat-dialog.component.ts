import { Component, OnInit } from '@angular/core';
import {ClientMsgDispatcher} from '../../services/client-msg-dispatcher.service';
import Message from '../../abstracts/message';

@Component({
  selector: 'chat-dialog',
  templateUrl: './chat-dialog.component.html',
  styleUrls: ['./dialog.component.sass']
})
export class ChatDialogComponent implements OnInit {

  messages: Message[] = [];

  constructor(private clientMsgDispatcher: ClientMsgDispatcher) { }

  ngOnInit(): void {
    this.clientMsgDispatcher.receive(msg => {
      this.messages.push(msg);
    });
  }

}
