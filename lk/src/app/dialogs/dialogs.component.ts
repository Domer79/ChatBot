import {Component, OnDestroy, OnInit} from '@angular/core';
import {Security} from "../security.decorator";
import {DialogService} from "../services/dialog.service";
import {Observable, Subscription} from "rxjs";
import MessageDialog from "../contracts/message-dialog";
import {MatDialog} from "@angular/material/dialog";
import {ClientChatDialogComponent} from "../client-chat-dialog/client-chat-dialog.component";

@Component({
  selector: 'app-dialogs',
  templateUrl: './dialogs.component.html',
  styleUrls: ['./dialogs.component.sass']
})
@Security('DialogPage')
export class DialogsComponent implements OnInit, OnDestroy {
  private dialogCreatedSubscription: Subscription;
  dialogs: Observable<MessageDialog[]>;

  constructor(
      private dialogService: DialogService,
      public dialog: MatDialog
  ) {
    this.dialogCreatedSubscription = this.dialogService.dialogCreated.subscribe(dialogId => {
      debugger;
      this.dialogs = this.dialogService.getDialogs();
    })
    this.dialogs = this.dialogService.getDialogs();
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.dialogCreatedSubscription.unsubscribe();
  }

  openChat() {
    const dialogRef = this.dialog.open(ClientChatDialogComponent, {
      width: '250px',
      data: { noData: 'noData' }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }
}
