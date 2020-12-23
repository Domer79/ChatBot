import {Component, Input, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import {animate, state, style, transition, trigger} from "@angular/animations";
import {Observable} from 'rxjs';
import Message from '../../abstracts/message';
import {ClientMsgDispatcher} from '../services/client-msg-dispatcher.service';
import {map, tap} from 'rxjs/operators';
import {PageDispatcherService} from '../services/page-dispatcher.service';
import {QuestionsComponent} from '../questions/questions/questions.component';

@Component({
  selector: 'msg-container',
  templateUrl: './msg-container.component.html',
  styleUrls: ['./msg-container.component.sass'],
  animations: [
    trigger('openClose', [
      state('open', style({
        opacity: 1,
        bottom: '70px'
      })),
      state('closed', style({
        opacity: 0,
        bottom: '-510px'
      })),
      transition('open => closed', [
        animate('0.3s ease-out')
      ]),
      transition('closed => open', [
        animate('0.3s cubic-bezier(0.35, 0, 0.25, 1)')
      ]),
    ])
  ]
})
export class MsgContainerComponent implements OnInit {
  @Input() opened = false;
  @Output() closed = new EventEmitter<void>();

  title = 'ChatBot';

  constructor(private pageDispatcher: PageDispatcherService) {

  }

  onClosed(): void{
    this.opened = false;
    this.closed.emit();
  }

  ngOnInit(): void{
    this.pageDispatcher.showPage(QuestionsComponent);
  }
}
