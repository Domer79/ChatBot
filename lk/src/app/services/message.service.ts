import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import Message from "../../abstracts/message";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(
      private httpClient: HttpClient
  ) { }

  getMessages(messageDialogId: string): Observable<Message[]>{
    return this.httpClient.get<Message[]>("api/Message/GetMessages", { params: {messageDialogId} });
  }
}
