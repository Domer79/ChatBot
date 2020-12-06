import {AfterViewInit, Component, ElementRef, Input, OnChanges, OnInit, SimpleChanges, ViewChild} from '@angular/core';
import Message from '../../abstracts/message';
import Helper from '../../misc/Helper';

@Component({
  selector: 'client-message',
  templateUrl: './client-message.component.html',
  styleUrls: ['./client-message.component.sass']
})
export class ClientMessageComponent implements OnInit, AfterViewInit {
  @Input() message: Message;
  @ViewChild('msg') msgView: ElementRef;
  constructor() { }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    this.msgView.nativeElement.innerHTML = this.message.content;
  }

  public get timeStatus(): string{
    return Helper.getTimeStatus(this.message.time);
  }
}
