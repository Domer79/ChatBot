import {Component, ElementRef, Inject, OnInit, ViewChild} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {ClientMessageService} from "../services/client-message.service";
import {MessageType} from "../../abstracts/message-type";
import Message from "../../abstracts/message";
import {TokenService} from "../services/token.service";
import MessageDialog, {DialogStatus} from "../contracts/message-dialog";
import {MessageService} from "../services/message.service";
import {map, tap} from "rxjs/operators";
import {Observable, Subject} from "rxjs";

@Component({
  selector: 'app-client-chat-dialog',
  templateUrl: './client-chat-dialog.component.html',
  styleUrls: ['./client-chat-dialog.component.sass']
})
export class ClientChatDialogComponent implements OnInit {
  inputText: string | null | undefined;
  messages: Message[] = [];
  private clientMessageService: ClientMessageService
  @ViewChild('chatEditor') chatEditorElement: ElementRef;
  private dialog: MessageDialog;
  private firstMessage$: Subject<Message> = new Subject<Message>();
  private firstMessage: Observable<Message> = this.firstMessage$.asObservable();
  senderName: Observable<string>;

  constructor(
      public dialogRef: MatDialogRef<ClientChatDialogComponent>,
      @Inject(MAT_DIALOG_DATA) public data: MessageDialog,
      private tokenService: TokenService,
      private messageService: MessageService
  ) {
    console.log(this.messageService);
    this.dialog = data;
    console.log(this.dialog.id);
    this.clientMessageService = new ClientMessageService(this.tokenService, data.id);
    this.clientMessageService.onConnected.subscribe(s => this.connectedEvent(s));
    this.clientMessageService.messages.subscribe(_ => this.messages.push(_));
    this.clientMessageService.meta.subscribe(m => this.setMeta(m));
    this.senderName = this.firstMessage.pipe(map(m => m.content));
  }

  ngOnInit(): void {
    this.dialogRef.afterClosed().subscribe(async result => {
      console.log('The dialog was closed');
      await this.clientMessageService.Disconnect();
    })
  }

  enter($event: KeyboardEvent): void {
    if (this.inputText === '' && ($event.code === 'Enter' && $event.shiftKey)){
      $event.preventDefault();
    }
    else if ($event.code === 'Enter' && $event.shiftKey){
      console.log(this.dialogRef);
      const clientHeight = this.chatEditorElement.nativeElement.clientHeight;
      this.chatEditorElement.nativeElement.style.height = `${clientHeight + 15}px`;
    }
  }

  onChange(eventTarget: EventTarget | any): void {
    this.inputText = eventTarget.innerHTML;
  }

  onKeydownEnter($event: Event): void {
    const event = $event as KeyboardEvent;
    if (this.inputText === ''){
      event.preventDefault();
      return;
    }

    if (!event.shiftKey)
    {
      this.sendMessage();
      $event.preventDefault();
    }
  }

  get isDisallowSend(){
    return this.dialog.dialogStatus == (DialogStatus.Closed | DialogStatus.Rejected)
        || !this.clientMessageService.connected;
  }

  sendMessageClick() {
    this.sendMessage();
  }

  private sendMessage(){
    if (this.inputText === ''){
      return;
    }

    this.clientMessageService.setMessage(MessageType.String, this.inputText);
    this.inputText = '';
    this.chatEditorElement.nativeElement.innerHTML = '';
    this.chatEditorElement.nativeElement.style.height = '';
  }

  private connectedEvent(connectionStatus: string): void {
    if (connectionStatus !== "success")
      return;

    this.messageService.getMessages(this.dialog.id).pipe(tap(messages => {
      if (!this.senderName){
        this.firstMessage$.next(messages[0]);
      }
    })).subscribe(m => this.messages = m.sort((a:Message, b: Message) => {
      if (a.time < b.time)
        return 1;

      if (a.time > b.time)
        return -1;

      return 0;
    }));
  }

  private setMeta(msg: Message) {
    const messages = this.messages.filter(_ => _.id === msg.id);
    if (messages.length > 0) {
      messages[0].status = msg.status;
    }
  }
}
