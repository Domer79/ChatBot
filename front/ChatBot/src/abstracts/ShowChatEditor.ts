import {Observable} from 'rxjs';

export interface ShowChatEditor{
  canShowEditor(): Observable<boolean>;
}
