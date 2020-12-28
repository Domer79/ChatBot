import {AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild} from '@angular/core';
import Message from "../../../abstracts/message";
import Helper from "../../../abstracts/Helper";

@Component({
  selector: 'operator-message',
  templateUrl: './operator-message.component.html',
  styleUrls: ['./operator-message.component.sass']
})
export class OperatorMessageComponent implements OnInit, AfterViewInit {
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
