import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {animate, state, style, transition, trigger} from '@angular/animations';
import {Subscription} from 'rxjs';
import {ClientMsgDispatcher} from '../services/client-msg-dispatcher.service';
import {PageDispatcherService} from '../services/page-dispatcher.service';
import {MainQuestionsComponent} from '../questions/main-questions/main-questions.component';
import {CommonService} from '../services/common.service';
import {MessageType} from '../../misc/message-type';
import Helper from '../../misc/Helper';

@Component({
  selector: 'msg-container',
  templateUrl: './msg-container.component.html',
  styleUrls: ['./msg-container.component.sass'],
  animations: [
    trigger('openClose', [
      state('open', style({
        opacity: 1,
        bottom: innerWidth < 600 || innerHeight < 700 ? '0' : '70px',
        top: 'inherit',
      })),
      state('closed', style({
        opacity: 0,
        bottom: innerHeight < 500 || innerWidth < 600 ? '-157%' : '-100%',
        top: innerHeight < 500 || innerWidth < 600 ? '100%' : 'inherit'
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

  private closeChatSubscription: Subscription;
  private closeSessionSubscription: Subscription;

  title = 'ChatBot';
  constructor(
    private pageDispatcher: PageDispatcherService,
    private common: CommonService,
    private clientMsgDispatcher: ClientMsgDispatcher
  ) {
    this.closeChatSubscription = this.pageDispatcher.getCloseChatEvent().subscribe(() => {
      this.closeChat();
    });
    this.closeSessionSubscription = this.common.closeSessionByTimeout.subscribe(_ => {
      clientMsgDispatcher.setMessage(MessageType.CloseSession, 'Диалог закрыт');
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
