import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import Message from '../../abstracts/message';
import {Observable} from 'rxjs';
import {ClientMsgDispatcher} from './client-msg-dispatcher.service';
import {NIL as guidEmpty, v4 as uuidv4} from 'uuid';
import {MessageOwner, MessageStatus, MessageType} from '../../misc/message-type';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(
    private httpClient: HttpClient,
  ) { }

  getMessages(messageDialogId: string): Observable<Message[]>{
    return this.httpClient.get<Message[]>('api/Message/GetMessagesForUser', {params: {messageDialogId}});
  }

  sendOfflineMessage(message: string, messageDialogId: string): Observable<Message>{
    const msg: Message = {
      id: uuidv4(),
      type: MessageType.String,
      content: message,
      owner: MessageOwner.client,
      status: MessageStatus.sending,
      time: new Date(),
      messageDialogId,
      questionId: null,
      question: null,
    };

    return this.httpClient.post<Message>('api/Message/AddOfflineMessage', msg);
  }
}
