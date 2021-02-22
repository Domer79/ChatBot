import { Injectable } from '@angular/core';
import {Observable, Subject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CommonService {
  private $opeCloseDialogFilter: Subject<boolean> = new Subject<boolean>();

  openCloseDialogFilter = this.$opeCloseDialogFilter.asObservable();

  constructor() { }

  toggleDialogFilter(newState: boolean): void{
    this.$opeCloseDialogFilter.next(newState);
  }
}
