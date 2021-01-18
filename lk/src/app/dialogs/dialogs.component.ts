import {Component, ElementRef, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {Security} from "../security.decorator";
import {DialogService} from "../services/dialog.service";
import {Observable, Subscription} from "rxjs";
import MessageDialog, {LinkType} from "../contracts/message-dialog";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {ClientChatDialogComponent} from "../client-chat-dialog/client-chat-dialog.component";
import {ActivatedRoute, Router} from "@angular/router";
import Page from "../contracts/Page";
import {PageEvent} from "@angular/material/paginator";
import {map, tap} from "rxjs/operators";
import {MatSnackBar} from "@angular/material/snack-bar";

@Component({
  selector: 'app-dialogs',
  templateUrl: './dialogs.component.html',
  styleUrls: ['./dialogs.component.sass']
})
@Security('DialogPage')
export class DialogsComponent implements OnInit, OnDestroy {
  @ViewChild('rla') rlaElement: ElementRef;
  activeLink = LinkType.all;
  linkType = LinkType;
  dialogPageSize: number = 10;
  dialogCurrentPage: number = 1;
  dialogPage: Page<MessageDialog>;
  dialogs: Observable<MessageDialog[]>;
  dialogCount: number;

  private dialogCreatedSubscription: Subscription;
  private paramsSubscription: Subscription;

  constructor(
      private dialogService: DialogService,
      private route: ActivatedRoute,
      public dialog: MatDialog,
      private router: Router,
      private activeRoute: ActivatedRoute,
      private matSnackBar: MatSnackBar,
  ) {
    this.activeLink = LinkType[route.snapshot.paramMap.get('id')];

    this.dialogCreatedSubscription = this.dialogService.dialogCreated.subscribe(dialogId => {
      this.matSnackBar.open('Создан диалог ', `/dialogs/${this.linkType.opened}`)
    });
  }

  updateDialogs(linkType?: LinkType){
    this.dialogs = this.dialogService.getDialogs(linkType ?? this.activeLink, this.dialogCurrentPage, this.dialogPageSize).pipe(tap(p => {
      this.dialogCount = p.totalCount;
    }), map(p => {
      return p.items
    }));
  }

  ngOnInit(): void {
    this.paramsSubscription = this.activeRoute.params.subscribe(p => {
      let pid: "all" | "opened" | "rejected" | "worked" | "closed" = p.id;
      this.activeLink = LinkType[pid];
      this.updateDialogs();
    })
  }

  ngOnDestroy(): void {
    this.dialogCreatedSubscription.unsubscribe();
    this.paramsSubscription.unsubscribe();
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
    await this.updateDialogs();
  }

  async reject(dlg: MessageDialog) {
    await this.dialogService.reject(dlg);
    await this.updateDialogs();
  }

  getPage($event: PageEvent) {
    console.log($event);
    this.dialogCurrentPage = $event.pageIndex + 1;
    this.dialogPageSize = $event.pageSize;
    this.updateDialogs();
  }
}
