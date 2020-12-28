import {Component, ElementRef, Inject, OnInit, ViewChild} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {ClientMessageService} from "../services/client-message.service";
import {MessageType} from "../../abstracts/message-type";
import Message from "../../abstracts/message";
import {TokenService} from "../services/token.service";
import MessageDialog, {DialogStatus} from "../contracts/message-dialog";
import {MessageService} from "../services/message.service";
import {tap} from "rxjs/operators";

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
  firstMessage: Message = undefined;

  constructor(
      public dialogRef: MatDialogRef<ClientChatDialogComponent>,
      @Inject(MAT_DIALOG_DATA) public data: MessageDialog,
      private tokenService: TokenService,
      private messageService: MessageService
  ) {
    debugger;
    console.log(this.messageService);
    this.dialog = data;
    console.log(this.dialog.id);
    this.clientMessageService = new ClientMessageService(this.tokenService, data.id);
    this.clientMessageService.onConnected.subscribe(s => this.connectedEvent(s));3
    this.clientMessageService.messages.subscribe(_ => this.messages.push(_));
  }

  ngOnInit(): void {
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

  onNoClick(): void {
    this.dialogRef.close();
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
    this.chatEditorElement.nativeElement.style.height = ``;
  }

  private connectedEvent(connectionStatus: string): void {
    if (connectionStatus !== "success")
      return;

    this.messageService.getMessages(this.dialog.id).pipe(tap(messages => {
      if (!this.firstMessage)
        this.firstMessage = messages[0];
    }))
        .subscribe(m => this.messages = m);
  }
}
