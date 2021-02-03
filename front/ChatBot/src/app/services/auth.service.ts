import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import User from '../../abstracts/User';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private httpClient: HttpClient
  ) { }

  sendAuthData(fio: string, email: string, phone: string): Observable<User>{
    return this.httpClient.post<User>('api/Auth/SaveAuthData', { fio, email, phone });
  }
}
