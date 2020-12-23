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

  constructor(
      private httpClient: HttpClient,
      private tokenService: TokenService
  ) {
    this.connection = new HubConnectionBuilder()
        .withUrl(`${environment.apiUrl}/dialog?token=${this.tokenService.tokenId}`,
            { accessTokenFactory: () => this.tokenService.tokenId ?? '' })
        .withAutomaticReconnect()
        .build();
    this.startConnection().then(() => {
      this.addDialogCreatedListener();
    })
  }

  startConnection(): Promise<void> {
    return this.connection.start();
  }

  addDialogCreatedListener = () => {
    this.connection.on('dialogCreated', (dialogId: string) => {
      debugger;
      this.dialogCreated$.next(dialogId);
    });
  }

  getDialogs(): Observable<MessageDialog[]>{
    return this.httpClient.get<MessageDialog[]>("api/Dialog/GetStartedDialogs");
  }
}
