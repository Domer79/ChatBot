import {AfterViewInit, Component, ElementRef, Input, OnChanges, OnInit, SimpleChanges, ViewChild} from '@angular/core';
import Message from '../../abstracts/message';

@Component({
  selector: 'client-message',
  templateUrl: './client-message.component.html',
  styleUrls: ['./client-message.component.sass']
})
export class ClientMessageComponent implements OnInit, OnChanges, AfterViewInit {
  @Input() message: Message;
  @ViewChild('msg') msgView: ElementRef;
  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    // const msg = changes.message.currentValue as Message;
    // this.msgView.nativeElement.innerHTML = msg.content;
  }

  ngAfterViewInit(): void {
    console.log(this.msgView);
    this.msgView.nativeElement.innerHTML = this.message.content;
  }

  public get timeStatus(): string{
    const now = new Date();
    // @ts-ignore
    let elapsed = now - this.message.time;
    elapsed = Math.round(elapsed / 1000 / 60);
    if (elapsed < 5)
      return 'только что';

    if (elapsed < 1440)
      return 'сегодня';

    if (elapsed < 2880)
      return 'Вчера';

    return '';
  }
}
