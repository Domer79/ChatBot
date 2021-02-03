import {Component, OnDestroy, OnInit} from '@angular/core';
import {QuestionsComponent} from '../questions/questions/questions.component';
import {PageDispatcherService} from '../services/page-dispatcher.service';
import {AuthService} from '../services/auth.service';
import User from '../../abstracts/User';
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-auth-form',
  templateUrl: './auth-form.component.html',
  styleUrls: ['./auth-form.component.sass']
})
export class AuthFormComponent implements OnInit, OnDestroy {
  fio = '';
  email = '';
  phone = '';
  private sendAuthDataSubscription: Subscription;

  constructor(
    private pageDispatcher: PageDispatcherService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
  }

  onCloseChat(): void {
    this.pageDispatcher.closeCurrent();
  }

  onOpenQuestions(): void {
    this.pageDispatcher.showPage(QuestionsComponent);
  }

  sendAuthData(): void {
    debugger;
    this.sendAuthDataSubscription = this.authService.sendAuthData(this.fio, this.email, this.phone).subscribe(_ => {
      console.log(_);
      this.pageDispatcher.closeCurrent();
    });
  }

  fioEnter($event: KeyboardEvent): void {
    if (this.fio === '' && ($event.code === 'Enter' && $event.shiftKey)){
      $event.preventDefault();
    }
  }

  fioInput(target: EventTarget | any): void {
    this.fio = target.innerHTML;
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

  ngOnDestroy(): void {
    this.sendAuthDataSubscription.unsubscribe();
  }
}
