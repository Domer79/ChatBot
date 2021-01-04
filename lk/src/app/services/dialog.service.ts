import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {HubConnection, HubConnectionBuilder} from "@microsoft/signalr";
import {environment} from "../../environments/environment";
import {BehaviorSubject, Observable, Subject} from "rxjs";
import MessageDialog from "../contracts/message-dialog";
import {TokenService} from "./token.service";

@Injectable({
  providedIn: 'root'
})
export class DialogService {
  private connection: HubConnection;
  private dialogCreated$: BehaviorSubject<string> = new BehaviorSubject<string>('');
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

  getDialogs(): Observable<MessageDialog[]>{
    return this.httpClient.get<MessageDialog[]>("api/Dialog/GetStartedOrActiveDialogs");
  }
}
