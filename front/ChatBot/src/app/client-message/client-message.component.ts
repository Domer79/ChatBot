import {AfterViewInit, Component, ElementRef, Input, OnChanges, OnInit, SimpleChanges, ViewChild} from '@angular/core';
import Message from '../../abstracts/message';
import Helper from '../../misc/Helper';
import {MessageType, TimeStatus} from '../../misc/message-type';

@Component({
  selector: 'client-message',
  templateUrl: './client-message.component.html',
  styleUrls: ['./client-message.component.sass']
})
export class ClientMessageComponent implements OnInit {
  @Input() message: Message;
  @ViewChild('msg') msgView: ElementRef;
  messageType = MessageType;

  constructor() { }

  ngOnInit(): void {
  }
}
