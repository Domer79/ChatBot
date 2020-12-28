import {EventEmitter, Injectable} from '@angular/core';
import {BehaviorSubject, Observable, Subject} from "rxjs";
import Message from "../../abstracts/message";
import {MessageType, MessageOwner, MessageStatus} from "../../abstracts/message-type";
import { v4 as uuidv4 } from 'uuid';
import { NIL as guidEmpty } from 'uuid';
import {HubConnection, HubConnectionBuilder, LogLevel} from "@microsoft/signalr";
import {environment} from "../../environments/environment";
import {TokenService} from "./token.service";

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
      messageDialogId: string
  ) {
    this.messages = this.messages$.asObservable();
    this.messageDialogId = messageDialogId;
    this.initHub();
  }

  private initHub(): void {
    const token = this.tokenService.tokenId;
    this.connection = new HubConnectionBuilder()
        .withUrl(`${environment.apiUrl}/chat?token=${token}`, { accessTokenFactory: () => {
            return token;
          }})
        .withAutomaticReconnect()
        .configureLogging(LogLevel.Information)
        .build();

    this.connectionInit();
    this.connection.start()
        .then(async r => {
          console.log('Connection started')
          await this.connection.invoke("OperatorConnect", this.messageDialogId);
        })
        .catch(err => console.log('Error while starting connection: ' + err));
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
      debugger;
      this.onConnected$.next(connectionStatus);
      this.connected = connectionStatus == "success";
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
}
