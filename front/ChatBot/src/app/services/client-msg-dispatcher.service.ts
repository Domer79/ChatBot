import {Injectable} from '@angular/core';
import {Observable, Subject} from 'rxjs';
import Message, {MessageInfo} from '../../abstracts/message';
import {SubscribeCallBack} from '../../misc/types';
import {MessageOwner, MessageStatus, MessageType} from '../../misc/message-type';
import {HubConnection, HubConnectionBuilder, LogLevel} from '@microsoft/signalr';
import {environment} from '../../environments/environment';
import {NIL as guidEmpty, v4 as uuidv4} from 'uuid';
import {TokenService} from './token.service';
import {MessageService} from './message.service';

@Injectable({
  providedIn: 'root'
})
export class ClientMsgDispatcher {
  private connection: HubConnection;
  private messages$: Subject<Message> = new Subject<Message>();
  private meta$: Subject<MessageInfo> = new Subject<MessageInfo>();
  public messages: Observable<Message> = this.messages$.asObservable();
  public meta: Observable<MessageInfo> = this.meta$.asObservable();
  private messageDialogId: string | null | undefined;

  constructor(
    private tokenService: TokenService,
    private messageService: MessageService,
  ) {
    this.initHub()
      .then()
      .catch(e => console.log(e));
  }

  private async initHub(): Promise<void> {
    const token = await this.tokenService.getToken();
    this.connection = new HubConnectionBuilder()
      .withUrl(`${environment.hubUrl}/chat?token=${token}`, { accessTokenFactory: () => {
          return token;
        }})
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();
    this.connection.serverTimeoutInMilliseconds = 120000;

    this.connectionInit();
    this.connection.start()
      .then(r => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err));
  }

  public setMessage(type: MessageType, msg: string): void{
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

  public loadMessages(): void{
    if (!this.messageDialogId) {
      return;
    }

    this.messageService.getMessages(this.messageDialogId).subscribe(m => {
      for (const item of m) {
        this.messages$.next(item);
      }
    });
  }

  private connectionInit(): void {
    this.connection.on('send', async (message: Message) => {
      message.status = MessageStatus.received;
      this.messages$.next(message);
      await this.connection.invoke('MessageRead', message);
    });

    this.connection.on('setMeta', (message: MessageInfo) => {
      this.messageDialogId = message.messageDialogId;
      this.meta$.next(message);
    });

    this.connection.on('dialogClosed', (message: Message) => {
      this.messages$.next({
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
