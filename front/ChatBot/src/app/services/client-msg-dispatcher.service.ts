import {Injectable} from '@angular/core';
import {Observable, Subject} from 'rxjs';
import Message from '../../abstracts/message';
import {SubscribeCallBack} from '../../misc/types';
import {MessageOwner, MessageStatus, MessageType} from '../../misc/message-type';
import {HubConnection, HubConnectionBuilder, LogLevel} from '@microsoft/signalr';
import {environment} from '../../environments/environment';
import { v4 as uuidv4 } from 'uuid';
import { NIL as guidEmpty } from 'uuid';
import {TokenService} from './token.service';

@Injectable({
  providedIn: 'root'
})
export class ClientMsgDispatcher {
  private connection: HubConnection;
  private messages$: Subject<Message> = new Subject<Message>();
  private meta$: Subject<Message> = new Subject<Message>();
  public messages: Observable<Message> = this.messages$.asObservable();
  public meta: Observable<Message> = this.meta$.asObservable();
  private messageDialogId: string | null | undefined;

  constructor(private tokenService: TokenService) {
    this.initHub()
      .then()
      .catch(e => console.log(e));
  }

  private async initHub(): Promise<void> {
    const token = await this.tokenService.getToken();
    this.connection = new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/chat?token=${token}`, { accessTokenFactory: () => {
          return token;
        }})
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();

    this.connectionInit();
    this.connection.start()
      .then(r => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err));
  }

  public receive(subscribeCallback: SubscribeCallBack): void {
    this.messages$.subscribe(subscribeCallback);
  }

  public stop(): void {
    this.messages$.complete();
  }

  public setMessage(type: MessageType, msg: string): void{
    debugger;
    const message: Message = {
      id: uuidv4(),
      type,
      content: msg,
      owner: MessageOwner.client,
      status: MessageStatus.sending,
      time: new Date(),
      messageDialogId: this.messageDialogId ?? guidEmpty,
    };

    this.connection.invoke('Send', message)
      .then(v => console.log(v))
      .catch(r => console.log(`Error: ${r}`));

    this.messages$.next(message);
  }

  private connectionInit(): void {
    this.connection.on('send', (message: Message) => {
      debugger;
      this.messages$.next(message);
    });

    this.connection.on('meta', (message: Message) => {
      this.messageDialogId = message.messageDialogId;
      this.meta$.next(message);
    });

    this.connection.on('closeDialog', (message: Message) => {
      this.meta$.next({
        id: uuidv4(),
        type: MessageType.String,
        content: 'Сессия закрыта',
        owner: MessageOwner.operator,
        status: MessageStatus.received,
        time: new Date(),
        messageDialogId: this.messageDialogId ?? guidEmpty,
      });
      this.messageDialogId = undefined;
    });
  }
}
