import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import Message from '../../abstracts/message';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(
    private httpClient: HttpClient
  ) { }

  getMessages(messageDialogId: string): Observable<Message[]>{
    return this.httpClient.get<Message[]>('api/Message/GetMessages', {params: {messageDialogId}});
  }
}
