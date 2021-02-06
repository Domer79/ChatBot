import {Component, ElementRef, OnDestroy, OnInit} from '@angular/core';
import {QuestionsComponent} from '../questions/questions/questions.component';
import {PageDispatcherService} from '../services/page-dispatcher.service';
import {AuthService} from '../services/auth.service';
import User from '../../abstracts/User';
import {Observable, Subscription} from 'rxjs';
import {CommonService} from '../services/common.service';
import {MessageService} from '../services/message.service';
import {ClientMsgDispatcher} from '../services/client-msg-dispatcher.service';
import Helper from '../../misc/Helper';
import {browser} from 'protractor';
import {concatAll, map, tap} from 'rxjs/operators';

@Component({
  selector: 'app-auth-form',
  templateUrl: './auth-form.component.html',
  styleUrls: ['./auth-form.component.sass']
})
export class AuthFormComponent implements OnInit, OnDestroy {
  fio = '';
  email = '';
  phone = '';
  message = '';
  private sendAuthDataSubscription: Subscription;
  private sendOfflineMessageSubscription: Subscription;
  private fioEditor: ElementRef;

  constructor(
    private pageDispatcher: PageDispatcherService,
    private authService: AuthService,
    private commonService: CommonService,
    private messageService: MessageService,
    private clientMsgDispatcher: ClientMsgDispatcher
  ) { }

  ngOnInit(): void {
  }

  onOpenQuestions(): void {
    this.pageDispatcher.showPage(QuestionsComponent);
  }

  sendAuthData(): void {
    this.sendAuthDataSubscription = this.authService.sendAuthData(this.fio, this.email, this.phone).subscribe(_ => {
      if (this.isShift){
        this.pageDispatcher.closeCurrent();
      }
    });
  }

  private sendOfflineMessage(): void {
    this.sendOfflineMessageSubscription = this.messageService.sendOfflineMessage(this.message, this.clientMsgDispatcher.messageDialogId)
      .subscribe(msg => {
        alert('Сообщение доставлено');
      });
  }

  sendData(): void{
    if (this.isShift){
      this.sendAuthData();
    }
    else{
      this.sendAuthData();
      this.sendOfflineMessage();
    }
  }

  fioEnter($event: KeyboardEvent): void {
    if (this.fio === '' && ($event.code === 'Enter' && $event.shiftKey)){
      $event.preventDefault();
    }
  }

  fioInput(target: EventTarget | any): void {
    this.fio = target.innerHTML;
    // debugger;
    // this.fio = Helper.searchQueryFilter(target.innerHTML);
    // if (this.fio === ''){
    //   this.removeBr(this.fioEditor);
    // }
  }

  fioOnKeydownEnter($event: any): void {
    const event = $event as KeyboardEvent;
    if (this.fio === ''){
      event.preventDefault();
      return;
    }

    if (!event.shiftKey)
    {
      $event.preventDefault();
    }
  }

  emailEnter($event: KeyboardEvent): void {
    if (this.email === '' && ($event.code === 'Enter' && $event.shiftKey)){
      $event.preventDefault();
    }
  }

  emailInput(target: EventTarget | any): void {
    this.email = target.innerHTML;
  }

  emailOnKeydownEnter($event: any): void {
    const event = $event as KeyboardEvent;
    if (this.email === ''){
      event.preventDefault();
      return;
    }

    if (!event.shiftKey)
    {
      $event.preventDefault();
    }
  }

  phoneEnter($event: KeyboardEvent): void {
    if (this.phone === '' && ($event.code === 'Enter' && $event.shiftKey)){
      $event.preventDefault();
    }
  }

  phoneInput(target: EventTarget | any): void {
    this.phone = target.innerHTML;
  }

  phoneOnKeydownEnter($event: any): void {
    const event = $event as KeyboardEvent;
    if (this.phone === ''){
      event.preventDefault();
      return;
    }

    if (!event.shiftKey)
    {
      $event.preventDefault();
    }
  }

  messageEnter($event: KeyboardEvent): void {
    if (this.message === '' && ($event.code === 'Enter' && $event.shiftKey)){
      $event.preventDefault();
    }
  }

  messageInput(target: EventTarget | any): void {
    this.message = target.innerHTML;
  }

  messageOnKeydownEnter($event: any): void {
    const event = $event as KeyboardEvent;
    if (this.message === ''){
      event.preventDefault();
      return;
    }

    if (!event.shiftKey)
    {
      $event.preventDefault();
    }
  }

  get isShift(): Observable<boolean>{
    return this.commonService.getShift().pipe(map(shift => {
      const now = new Date();
      const beginDate = new Date(now.getFullYear(), now.getMonth(), now.getDate(), shift.begin, 0, 0);
      const closeDate = new Date(now.getFullYear(), now.getMonth(), now.getDate(), shift.close, 0, 0);
      return beginDate < now && now < closeDate;
    }));
  }

  ngOnDestroy(): void {
    if (this.sendAuthDataSubscription){
      this.sendAuthDataSubscription.unsubscribe();
    }

    if (this.sendOfflineMessageSubscription){
      this.sendOfflineMessageSubscription.unsubscribe();
    }
  }

  private removeBr(element: ElementRef): void{
    debugger;
    element.nativeElement.querySelector('br');
  }
}
