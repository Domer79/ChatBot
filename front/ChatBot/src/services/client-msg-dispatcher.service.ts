import {Injectable} from '@angular/core';
import {Observable, Subject} from 'rxjs';
import Message from '../abstracts/message';
import {SubscribeCallBack} from '../misc/types';
import {MessageOwner, MessageStatus, MessageType} from '../misc/message-type';

@Injectable({
  providedIn: 'root'
})
export class ClientMsgDispatcher {
  private messages$: Subject<Message> = new Subject<Message>();
  public messages: Observable<Message> = this.messages$.asObservable();

  constructor() {
  }

  public receive(subscribeCallback: SubscribeCallBack): void {
    this.messages$.subscribe(subscribeCallback);
  }

  public stop(): void {
    this.messages$.complete();
  }

  public setMessage(type: MessageType, msg: string): void{
    this.messages$.next({ type, content: msg, owner: MessageOwner.client, status: MessageStatus.sending, time: new Date() });
    this.messages$.next({ type, content: msg, owner: MessageOwner.operator, status: MessageStatus.sending, time: new Date() });
  }
}
