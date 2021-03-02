import {BehaviorSubject, Observable, Subject} from "rxjs";
import Message from "../../abstracts/message";
import {MessageOwner, MessageStatus, MessageType} from "../../abstracts/message-type";
import {NIL as guidEmpty, v4 as uuidv4} from 'uuid';
import {HubConnection, HubConnectionBuilder, LogLevel} from "@microsoft/signalr";
import {environment} from "../../environments/environment";
import {TokenService} from "./token.service";
import {MessageService} from "./message.service";
import {map} from "rxjs/operators";

export class ClientMessageService {
  private connection: HubConnection;
  private messages$: Subject<Message> = new Subject<Message>();
  private messageDialogId: string | null | undefined;
  public messages: Observable<Message>
  private meta$: Subject<Message> = new Subject<Message>();
  public meta: Observable<Message> = this.meta$.asObservable();
  public connected: boolean = false;
  public onConnected$: BehaviorSubject<any> = new BehaviorSubject<any>("");
  public onConnected: Observable<any> = this.onConnected$.asObservable();

  constructor(
      private tokenService: TokenService,
      private messageService: MessageService,
      messageDialogId: string
  ) {
    this.messages = this.messages$.asObservable();
    this.messageDialogId = messageDialogId;
    this.initHub();
  }

  private initHub(): void {
    const token = this.tokenService.tokenId;
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
        .then(async r => {
          console.log('Connection started')
          await this.connection.invoke("OperatorConnect", this.messageDialogId);
        })
        .catch(err => console.log('Error while starting connection: ' + err));
  }

  private connectionInit(): void {
    this.connection.on('send', async (message: Message) => {
      message.status = MessageStatus.received;
      this.messages$.next(message);
      await this.connection.invoke('MessageRead', message);
    });

    this.connection.on('setMeta', (message: Message) => {
      this.messageDialogId = message.messageDialogId;
      this.meta$.next(message);
    });

    this.connection.on('closeDialog', (message: Message) => {
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

    this.connection.on("operatorConnect", (connectionStatus: string) => {
      this.onConnected$.next(connectionStatus);
      this.connected = connectionStatus == "success";

      this.messageService.getMessages(this.messageDialogId).pipe(map(msgs => {
        return msgs.sort((a:Message, b: Message) => {
          if (a.time < b.time)
            return -1;

          if (a.time > b.time)
            return 1;

          return 0;
        })
      })).subscribe(async msgs => {
        debugger
        for (const msg of msgs){
          this.messages$.next(msg);
          if (msg.status !== MessageStatus.received){
            msg.status = MessageStatus.received;
            await this.connection.invoke('MessageRead', msg);
          }
        }
      });
    })
  }

  public setMessage(type: MessageType, msg: string): void{
    const message: Message = {
      id: uuidv4(),
      type,
      content: msg,
      owner: MessageOwner.operator,
      status: MessageStatus.sending,
      time: new Date(),
      messageDialogId: this.messageDialogId ?? guidEmpty,
    };

    this.connection.invoke('Send', message)
        .then(v => console.log(v))
        .catch(r => console.log(`Error: ${r}`));

    this.messages$.next(message);
  }

  public async Disconnect(): Promise<void>{
    await this.connection.invoke('OperatorDisconnect', this.messageDialogId);
  }
}
