import { Injectable } from '@angular/core';
import {BehaviorSubject, Observable, Subject} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChatManagerService {
  private isOpenChat$: Subject<boolean> = new Subject<boolean>();
  isOpenChat: Observable<boolean> = this.isOpenChat$.asObservable();

  constructor() { }

  public setOpenChat(isOpen: boolean): void{
    this.isOpenChat$.next(isOpen);
  }
}
