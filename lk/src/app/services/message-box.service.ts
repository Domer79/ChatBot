import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {DialogResult} from "../../abstracts/DialogResult";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {ConfirmComponent} from "../dialogs/confirm/confirm.component";
import {map} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class MessageBoxService {

  constructor(
      private matDialog: MatDialog
  ) { }

  confirm(message: string): Observable<DialogResult>{
    const config = new MatDialogConfig();
    config.width = '500px';
    config.height = '300px';
    config.data = message;
    return this.matDialog.open(ConfirmComponent, config).afterClosed().pipe(map(_ => _ as DialogResult));
  }
}
