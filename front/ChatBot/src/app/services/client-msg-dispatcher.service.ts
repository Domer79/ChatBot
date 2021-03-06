import {Injectable, OnDestroy} from '@angular/core';
import {Observable, Subject, Subscription} from 'rxjs';
import Message, {MessageInfo} from '../../abstracts/message';
import {MessageOwner, MessageStatus, MessageType} from '../../misc/message-type';
import {HubConnection, HubConnectionBuilder, LogLevel} from '@microsoft/signalr';
import {environment} from '../../environments/environment';
import {NIL as guidEmpty, v4 as uuidv4} from 'uuid';
import {TokenService} from './token.service';
import {MessageService} from './message.service';
import {CommonService} from './common.service';

@Injectable({
  providedIn: 'root'
})
export class ClientMsgDispatcher implements OnDestroy{
  private connection: HubConnection;
  private messages$: Subject<Message> = new Subject<Message>();
  private meta$: Subject<MessageInfo> = new Subject<MessageInfo>();
  public messages: Observable<Message> = this.messages$.asObservable();
  public meta: Observable<MessageInfo> = this.meta$.asObservable();
  messageDialogId: string | null | undefined;
  private serverTimeoutInMillisecondsSubscription: Subscription;

  constructor(
    private tokenService: TokenService,
    private messageService: MessageService,
    private common: CommonService,
  ) {
    this.initHub()
      .then()
      .catch(e => console.log(e));
  }

  ngOnDestroy(): void {
    this.serverTimeoutInMillisecondsSubscription.unsubscribe();
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

    this.serverTimeoutInMillisecondsSubscription = this.common.serverTimeoutInMilliseconds().subscribe(_ => {
      this.connection.serverTimeoutInMilliseconds = _;
      this.connection.keepAliveIntervalInMilliseconds = _ / 2;

      this.connection.onclose(err => {
        console.log('Connection close');
      });

      this.connectionInit();
      this.connection.start()
        .then(r => console.log('Connection started'))
        .catch(err => console.log('Error while starting connection: ' + err));
    });
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
      questionId: null,
      question: null,
    };

    this.connection.invoke('Send', message)
      .then(v => console.log(v))
      .catch(r => console.log(`Error: ${r}`));

    this.messages$.next(message);
    this.common.restartTimeout();
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

      if (message.type === MessageType.CloseSession){
        return;
      }

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
        questionId: null,
        question: null,
      });
      this.messageDialogId = undefined;
    });

    this.connection.on('sendQuestions', (messages: Message[]) => {
      for (const message of messages){
        this.messages$.next(message);
      }
    });

    this.connection.on('sendButton', (message: Message) => {
      this.messages$.next(message);
    });
  }
}
