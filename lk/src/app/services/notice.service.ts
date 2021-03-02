import { Injectable } from '@angular/core';
import {MatSnackBar} from "@angular/material/snack-bar";

@Injectable({
  providedIn: 'root'
})
export class NoticeService {

  constructor(
      private snackBar: MatSnackBar
  ) { }

  notice(message: string): void{
    this.snackBar.open(message, 'Закрыть', { duration: 5000 });
  }
}
