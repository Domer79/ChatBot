import {Component, OnDestroy, OnInit} from '@angular/core';
import {Security} from "../security.decorator";
import {DialogService} from "../services/dialog.service";
import {merge, Observable, of, Subscription} from "rxjs";
import MessageDialog, {DialogStatus, LinkType} from "../contracts/message-dialog";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {ClientChatDialogComponent} from "../client-chat-dialog/client-chat-dialog.component";
import {catchError} from "rxjs/operators";
import {ActivatedRoute, Router} from "@angular/router";
import Page from "../contracts/Page";
import {PageEvent} from "@angular/material/paginator";

@Component({
  selector: 'app-dialogs',
  templateUrl: './dialogs.component.html',
  styleUrls: ['./dialogs.component.sass']
})
@Security('DialogPage')
export class DialogsComponent implements OnInit, OnDestroy {
  activeLink = LinkType.all;
  linkType = LinkType;

  private dialogCreatedSubscription: Subscription;

  dialogPageSize: number = 10;
  dialogCurrentPage: number = 1;
  dialogPage: Page<MessageDialog>;
  dialogs: MessageDialog[];
  dialogCount: number;
  private status: DialogStatus;

  constructor(
      private dialogService: DialogService,
      private route: ActivatedRoute,
      public dialog: MatDialog,
      private router: Router,
  ) {
    this.status = Number(route.snapshot.paramMap.get('id')) || LinkType.all;

    this.dialogService.getDialogs(this.status, this.dialogCurrentPage, this.dialogPageSize).subscribe(p => {
      this.dialogCount = p.totalCount;
      this.dialogs = p.items;
    });

    this.dialogCreatedSubscription = this.dialogService.dialogCreated.subscribe(dialogId => {
      this.dialogService.getDialogs(this.status, this.dialogCurrentPage, this.dialogPageSize).subscribe(p => {
        this.dialogCount = p.totalCount;
        this.dialogs = p.items;
      });
    });
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.dialogCreatedSubscription.unsubscribe();
  }

  openChat(messageDialog: MessageDialog) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = '500px';
    dialogConfig.height = '510px';
    dialogConfig.data = messageDialog;
    this.dialog.open(ClientChatDialogComponent, dialogConfig);
  }

  async activate(dlg: MessageDialog) {
    await this.dialogService.activate(dlg);
    const p = await this.dialogService.getDialogs(this.status, this.dialogCurrentPage, this.dialogPageSize).toPromise();
    this.dialogCount = p.totalCount;
    this.dialogs = p.items;
  }

  async reject(dlg: MessageDialog) {
    await this.dialogService.reject(dlg);
    const p = await this.dialogService.getDialogs(this.status, this.dialogCurrentPage, this.dialogPageSize).toPromise();
    this.dialogCount = p.totalCount;
    this.dialogs = p.items;
  }

  async goTo(linkType: LinkType) {
    switch (linkType) {
      case LinkType.all:{
        this.activeLink = LinkType.all;
        await this.router.navigate(['dialogs'], { skipLocationChange: true });
        break;
      }
      case LinkType.opened:{
        this.activeLink = LinkType.opened;
        await this.router.navigate([`dialogs/${LinkType.opened}`], { skipLocationChange: true });
        break;
      }
      case LinkType.worked:{
        this.activeLink = LinkType.worked;
        await this.router.navigate([`dialogs/${LinkType.worked}`], { skipLocationChange: true });
        break;
      }
      case LinkType.rejected:{
        this.activeLink = LinkType.rejected;
        await this.router.navigate([`dialogs/${LinkType.rejected}`], { skipLocationChange: true });
        break;
      }
      case LinkType.closed:{
        this.activeLink = LinkType.closed;
        await this.router.navigate([`dialogs/${LinkType.closed}`], { skipLocationChange: true });
        break;
      }
    }
  }

  getPage($event: PageEvent) {

  }
}
