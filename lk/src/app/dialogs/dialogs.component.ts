import {Component, OnDestroy, OnInit} from '@angular/core';
import {Security} from "../security.decorator";
import {DialogService} from "../services/dialog.service";
import {merge, Observable, of, Subscription} from "rxjs";
import MessageDialog from "../contracts/message-dialog";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {ClientChatDialogComponent} from "../client-chat-dialog/client-chat-dialog.component";
import {catchError} from "rxjs/operators";

@Component({
  selector: 'app-dialogs',
  templateUrl: './dialogs.component.html',
  styleUrls: ['./dialogs.component.sass']
})
@Security('DialogPage')
export class DialogsComponent implements OnInit, OnDestroy {
  private dialogCreatedSubscription: Subscription;
  private dialogClosedSubscription: Subscription;

  dialogs: Observable<MessageDialog[]>;

  constructor(
      private dialogService: DialogService,
      public dialog: MatDialog
  ) {
    this.dialogCreatedSubscription = this.dialogService.dialogCreated.pipe(catchError(err => {
      debugger;
      console.error(err);
      return of(err);
    })).subscribe(dialogId => {
      this.dialogs = this.dialogService.getDialogs();
    });
    this.dialogClosedSubscription = this.dialogService.dialogClosed.pipe(catchError(err => {
      debugger;
      console.error(err);
      return of(err);
    })).subscribe(dialogId => {
      this.dialogs = this.dialogService.getDialogs();
    });
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.dialogCreatedSubscription.unsubscribe();
    this.dialogClosedSubscription.unsubscribe();
  }

  openChat(messageDialog: MessageDialog) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = '500px';
    dialogConfig.height = '510px';
    dialogConfig.data = messageDialog;
    this.dialog.open(ClientChatDialogComponent, dialogConfig);
  }
}
