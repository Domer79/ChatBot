import {Component, OnDestroy, OnInit} from '@angular/core';
import {Security} from "../security.decorator";
import {DialogService} from "../services/dialog.service";
import {merge, Observable, Subscription} from "rxjs";
import MessageDialog from "../contracts/message-dialog";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {ClientChatDialogComponent} from "../client-chat-dialog/client-chat-dialog.component";

@Component({
  selector: 'app-dialogs',
  templateUrl: './dialogs.component.html',
  styleUrls: ['./dialogs.component.sass']
})
@Security('DialogPage')
export class DialogsComponent implements OnInit, OnDestroy {
  private dialogEventsSubscription: Subscription;
  dialogs: Observable<MessageDialog[]>;

  constructor(
      private dialogService: DialogService,
      public dialog: MatDialog
  ) {
    this.dialogEventsSubscription = merge(this.dialogService.dialogCreated, this.dialogService.dialogClosed).subscribe(dialogId => {
      debugger;
      this.dialogs = this.dialogService.getDialogs();
    })
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.dialogEventsSubscription.unsubscribe();
  }

  openChat(messageDialog: MessageDialog) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = '500px';
    dialogConfig.height = '510px';
    dialogConfig.data = messageDialog;
    const dialogRef = this.dialog.open(ClientChatDialogComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }
}
