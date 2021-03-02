import {Component, ElementRef, Inject, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {ClientMessageService} from "../services/client-message.service";
import {MessageType} from "../../abstracts/message-type";
import Message from "../../abstracts/message";
import {TokenService} from "../services/token.service";
import MessageDialog, {DialogStatus} from "../contracts/message-dialog";
import {MessageService} from "../services/message.service";
import {map} from "rxjs/operators";
import {Observable, Subject, Subscription} from "rxjs";
import Helper from "../misc/Helper";

@Component({
  selector: 'app-client-chat-dialog',
  templateUrl: './client-chat-dialog.component.html',
  styleUrls: ['./client-chat-dialog.component.sass']
})
export class ClientChatDialogComponent implements OnInit, OnDestroy {
  private clientMessageService: ClientMessageService
  private firstMessage$: Subject<Message> = new Subject<Message>();
  private firstMessage: Observable<Message> = this.firstMessage$.asObservable();
  private onConnectedSubscription: Subscription;
  private messagesSubscription: Subscription;
  private metaSubscription: Subscription;
  private clientMessageServiceSubscription: Subscription;

  inputText: string | null | undefined;
  messages: Message[] = [];
  @ViewChild('chatEditor') chatEditorElement: ElementRef;
  senderName: Observable<string>;
  dialog: MessageDialog;
  dialogStatus = DialogStatus;

  constructor(
      public dialogRef: MatDialogRef<ClientChatDialogComponent>,
      @Inject(MAT_DIALOG_DATA) public data: MessageDialog,
      private tokenService: TokenService,
      private messageService: MessageService
  ) {
    this.dialog = data;
    this.clientMessageService = new ClientMessageService(this.tokenService, this.messageService, data.id);
    this.onConnectedSubscription = this.clientMessageService.onConnected.subscribe(s => ClientChatDialogComponent.connectedEvent(s));
    this.messagesSubscription = this.clientMessageService.messages.subscribe(_ => this.messages.push(_));
    this.metaSubscription = this.clientMessageService.meta.subscribe(m => this.setMeta(m));
    this.clientMessageServiceSubscription = this.clientMessageService.connectionStartError.subscribe(_ => {
      if (_.exception !== 'DialogNotActiveException'){
        return;
      }

      this.messageService.getMessages(data.id).subscribe(_ => {
        this.messages = Helper.sortMessages(_);
      });
    });
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
    return this.dialog.dialogStatus !== DialogStatus.Active
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

  private static connectedEvent(connectionStatus: string): void {
    if (connectionStatus !== "success")
      return;
  }

  private setMeta(msg: Message) {
    const messages = this.messages.filter(_ => _.id === msg.id);
    if (messages.length > 0) {
      messages[0].status = msg.status;
    }
  }

  closeDialog() {
    this.clientMessageService.setMessage(MessageType.CloseSession, "Диалог закрыт");
    this.dialogRef.close();
  }

  ngOnDestroy(): void {
    this.messagesSubscription.unsubscribe();
    this.onConnectedSubscription.unsubscribe();
    this.metaSubscription.unsubscribe();
    this.clientMessageServiceSubscription.unsubscribe();
  }
}
