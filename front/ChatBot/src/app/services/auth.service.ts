import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import User from '../../abstracts/User';
import {Observable, Subject} from 'rxjs';
import {map, tap} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly user$: Subject<User>;
  private readonly user: Observable<User>;

  constructor(
    private httpClient: HttpClient
  ) {
    this.user$ = new Subject<User>();
    this.user = this.user$.asObservable();
  }

  sendAuthData(fio: string, email: string, phone: string): Observable<User>{
    return this.httpClient.post<User>('api/Auth/SaveAuthData', { fio, email, phone }).pipe(tap(user => {
      this.user$.next(user);
    }));
  }

  isActive(): Observable<boolean>{
    return this.user.pipe(map(user => {
      return user.isActive;
    }));
  }
}
