import {AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {ClientMsgDispatcher} from '../services/client-msg-dispatcher.service';
import {MessageType} from '../../misc/message-type';
import {PageDispatcherService} from '../services/page-dispatcher.service';
import {QuestionsComponent} from '../questions/questions/questions.component';
import {forkJoin, Observable, of, Subscription, combineLatest} from 'rxjs';
import {delay, map, switchMap, timeout} from 'rxjs/operators';
import {ShowChatEditor} from '../../abstracts/ShowChatEditor';
import {AuthService} from '../services/auth.service';
import {ChatEditorService} from '../services/chat-editor.service';
import {ChatDialogComponent} from '../dialog/chat-dialog.component';
import {ChatManagerService} from '../services/chat-manager.service';

@Component({
  selector: 'chat-editor',
  templateUrl: './chat-editor.component.html',
  styleUrls: ['./chat-editor.component.sass']
})
export class ChatEditorComponent implements OnInit, OnDestroy, AfterViewInit {
  private originalClientHeight: number;
  private closeCurrentTimeoutSubscription: Subscription;
  private isOpenSubscription: Subscription;
  private isFixed = false;

  message = '';
  isOpen = false;

  @ViewChild('editor') editorElement: ElementRef;
  @ViewChild('chatEditor') chatEditorElement: ElementRef;

  constructor(
    private clientMsgDispatcher: ClientMsgDispatcher,
    private pageDispatcher: PageDispatcherService,
    private chatEditorService: ChatEditorService,
    private chatManagerService: ChatManagerService,
    ) {
    this.isOpenSubscription = this.chatManagerService.isOpenChat.subscribe(val => this.isOpen = val);
  }

  ngOnInit(): void {
  }

  get canShow(): Observable<boolean>{
    return this.chatEditorService.canShow();
  }

  enter($event: KeyboardEvent): void {
    if (this.message === '' && ($event.code === 'Enter' && $event.shiftKey)){
      $event.preventDefault();
    }
    else if ($event.code === 'Enter' && $event.shiftKey){
      const clientHeight = this.chatEditorElement.nativeElement.clientHeight;
      this.chatEditorElement.nativeElement.style.height = `${clientHeight + 15}px`;
    }
  }

  onChange(eventTarget: EventTarget | any): void {
    this.message = eventTarget.innerHTML;
  }

  onKeyupEnter($event: Event): void {
  }

  onKeydownEnter($event: Event): void {
    const event = $event as KeyboardEvent;
    if (this.message === ''){
      event.preventDefault();
      return;
    }

    if (!event.shiftKey)
    {
      this.sendMessage();
      $event.preventDefault();
    }
  }

  ngAfterViewInit(): void {
    this.isFixed = innerHeight < 500;
    this.originalClientHeight = this.chatEditorElement?.nativeElement?.clientHeight ?? 50;
  }

  sendMessage(): void {
    this.clientMsgDispatcher.setMessage(MessageType.String, this.message);
    this.message = '';
    this.editorElement.nativeElement.innerHTML = '';
    this.chatEditorElement.nativeElement.style.height = `${this.originalClientHeight}px`;

    if (this.pageDispatcher.getCurrent().componentName === 'MainQuestionsComponent'){
      this.closeCurrentTimeoutSubscription = of(1).pipe(delay(350)).subscribe(() => {
        this.pageDispatcher.showPage(ChatDialogComponent);
      });
    }
  }

  ngOnDestroy(): void {
    if (this.closeCurrentTimeoutSubscription){
      this.closeCurrentTimeoutSubscription.unsubscribe();
    }

    if (this.isOpenSubscription){
      this.isOpenSubscription.unsubscribe();
    }
  }
}
