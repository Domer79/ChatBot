import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {HubConnection, HubConnectionBuilder} from "@microsoft/signalr";
import {environment} from "../../environments/environment";
import {BehaviorSubject, Observable, Subject} from "rxjs";
import MessageDialog, {DialogStatus} from "../contracts/message-dialog";
import {TokenService} from "./token.service";
import Message from "../../abstracts/message";
import Page from "../contracts/Page";

@Injectable({
  providedIn: 'root'
})
export class DialogService {
  private connection: HubConnection;
  private dialogCreated$: Subject<string> = new Subject<string>();
  public dialogCreated: Observable<string> = this.dialogCreated$.asObservable();
  private dialogClosed$: Subject<string> = new Subject<string>();
  public dialogClosed: Observable<string> = this.dialogClosed$.asObservable();

  constructor(
      private httpClient: HttpClient,
      private tokenService: TokenService
  ) {
    this.connection = new HubConnectionBuilder()
        .withUrl(`${environment.hubUrl}/dialog?token=${this.tokenService.tokenId}`,
            { accessTokenFactory: () => this.tokenService.tokenId ?? '' })
        .withAutomaticReconnect()
        .build();
    this.connection.serverTimeoutInMilliseconds = 120000;

    this.startConnection().then(() => {
      this.addDialogCreatedListener();
    });
  }

  startConnection(): Promise<void> {
    return this.connection.start();
  }

  addDialogCreatedListener = () => {
    this.connection.on('dialogCreated', (dialogId: string) => {
      this.dialogCreated$.next(dialogId);
    });

    this.connection.on('dialogClosed', (dialogId: string) => {
      this.dialogClosed$.next(dialogId);
    });
  }

  getMessages(messageDialogId: string): Observable<Message[]>{
    return this.httpClient.get<Message[]>("api/Message/GetMessages", { params: {messageDialogId} });
  }

  getDialogs(dialogStatus: number, page: number, size: number): Observable<Page<MessageDialog>>{
    return this.httpClient.get<Page<MessageDialog>>(
        `api/Dialog/GetDialogsByStatus?status=${dialogStatus}&number=${page}&size=${size}`);
  }

  async activate(dlg: MessageDialog) {
    await this.httpClient.post("api/Dialog/Activate", { messageDialogId: dlg.id }).toPromise();
  }

  async reject(dlg: MessageDialog) {
    await this.httpClient.post("api/Dialog/Reject", { messageDialogId: dlg.id }).toPromise();
  }
}
