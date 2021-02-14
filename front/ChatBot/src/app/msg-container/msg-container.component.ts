import {Component, Input, Output, EventEmitter, OnInit, ElementRef, OnDestroy} from '@angular/core';
import {animate, state, style, transition, trigger} from "@angular/animations";
import {Observable, Subscription} from 'rxjs';
import Message from '../../abstracts/message';
import {ClientMsgDispatcher} from '../services/client-msg-dispatcher.service';
import {map, tap} from 'rxjs/operators';
import {PageDispatcherService} from '../services/page-dispatcher.service';
import {QuestionsComponent} from '../questions/questions/questions.component';
import {MainQuestionsComponent} from '../questions/main-questions/main-questions.component';
import {CommonService} from '../services/common.service';

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
        bottom: '-640px'
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
export class MsgContainerComponent implements OnInit, OnDestroy {
  @Input() opened = false;
  @Output() closed = new EventEmitter<void>();

  title = 'ChatBot';
  private closeChatSubscription: Subscription;
  private closeSessionSubscription: Subscription;

  constructor(
    private pageDispatcher: PageDispatcherService,
    private common: CommonService
  ) {
    this.closeChatSubscription = this.pageDispatcher.getCloseChatEvent().subscribe(() => {
      this.closeChat();
    });
    this.closeSessionSubscription = this.common.closeSessionByTimeout.subscribe(_ => {
      this.pageDispatcher.closeChatFull();
    });
  }

  closeChat(): void{
    this.opened = false;
    this.closed.emit();
  }

  ngOnInit(): void{
    this.pageDispatcher.showPage(MainQuestionsComponent);
  }

  ngOnDestroy(): void {
    this.closeChatSubscription.unsubscribe();
    this.closeSessionSubscription.unsubscribe();
  }
}
