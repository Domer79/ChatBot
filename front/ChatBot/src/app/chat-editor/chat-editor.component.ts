import {AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {ClientMsgDispatcher} from '../services/client-msg-dispatcher.service';
import {MessageType} from '../../misc/message-type';
import {PageDispatcherService} from '../services/page-dispatcher.service';
import {QuestionsComponent} from '../questions/questions/questions.component';
import {Observable, of, Subscription} from 'rxjs';
import {timeout} from 'rxjs/operators';
import {ShowChatEditor} from '../../abstracts/ShowChatEditor';
import {AuthService} from '../services/auth.service';

@Component({
  selector: 'chat-editor',
  templateUrl: './chat-editor.component.html',
  styleUrls: ['./chat-editor.component.sass']
})
export class ChatEditorComponent implements OnInit, OnDestroy, AfterViewInit {
  private originalClientHeight: number;
  private closeCurrentTimeoutSubscription: Subscription;

  message = '';

  @ViewChild('editor') editorElement: ElementRef;
  @ViewChild('chatEditor') chatEditorElement: ElementRef;

  constructor(
    private clientMsgDispatcher: ClientMsgDispatcher,
    private pageDispatcher: PageDispatcherService,
    ) {
  }

  ngOnInit(): void {
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
    this.originalClientHeight = this.chatEditorElement.nativeElement.clientHeight;
  }

  sendMessage(): void {
    this.clientMsgDispatcher.setMessage(MessageType.String, this.message);
    this.message = '';
    this.editorElement.nativeElement.innerHTML = '';
    this.chatEditorElement.nativeElement.style.height = `${this.originalClientHeight}px`;

    if (this.pageDispatcher.getCurrent().componentName === 'MainQuestionsComponent'){
      this.closeCurrentTimeoutSubscription = of(1).pipe(timeout(300)).subscribe(() => {
        this.pageDispatcher.closeCurrent();
      });
    }
  }

  ngOnDestroy(): void {
    if (this.closeCurrentTimeoutSubscription){
      this.closeCurrentTimeoutSubscription.unsubscribe();
    }
  }
}
