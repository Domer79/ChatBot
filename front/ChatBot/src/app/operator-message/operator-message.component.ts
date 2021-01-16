import {AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild} from '@angular/core';
import Message from '../../abstracts/message';
import Helper from '../../misc/Helper';
import {TimeStatus} from '../../misc/message-type';

@Component({
  selector: 'operator-message',
  templateUrl: './operator-message.component.html',
  styleUrls: ['./operator-message.component.sass']
})
export class OperatorMessageComponent implements AfterViewInit {
  @Input() message: Message;
  @ViewChild('msg') msgView: ElementRef;
  constructor() { }

  ngAfterViewInit(): void {
    this.msgView.nativeElement.innerHTML = this.message.content;
  }
}
