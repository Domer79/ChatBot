import {Component, ElementRef, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {Security} from "../security.decorator";
import {DialogService} from "../services/dialog.service";
import {Observable, of, Subscription} from "rxjs";
import MessageDialog, {DialogStatus, LinkType} from "../contracts/message-dialog";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {ClientChatDialogComponent} from "../client-chat-dialog/client-chat-dialog.component";
import {ActivatedRoute, Router} from "@angular/router";
import Page from "../contracts/Page";
import {PageEvent} from "@angular/material/paginator";
import {map, switchMap, tap} from "rxjs/operators";
import {DialogFilterService} from "../services/dialog-filter.service";
import Helper from "../misc/Helper";
import {DialogFilterData} from "../../abstracts/dialog-filter-data";
import * as _moment from 'moment';
// tslint:disable-next-line:no-duplicate-imports
// @ts-ignore
import {default as _rollupMoment} from 'moment';

const moment = _rollupMoment || _moment;

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
  dialogStatus = DialogStatus;
  dialogPageSize: number = 10;
  dialogCurrentPage: number = 1;
  dialogPage: Page<MessageDialog>;
  dialogs: Observable<MessageDialog[]>;
  dialogCount: number;

  private dialogCreatedSubscription: Subscription;
  private paramsSubscription: Subscription;
  private dialogClosedSubscription: Subscription;
  private dialogFilterSubscription: Subscription;
  private queryParamsSubscription: Subscription;
  private dialogFilterData: DialogFilterData;

  constructor(
      private dialogService: DialogService,
      private route: ActivatedRoute,
      public dialog: MatDialog,
      private router: Router,
      private activeRoute: ActivatedRoute,
      private dialogFilterService: DialogFilterService,
  ) {
    this.activeLink = LinkType[route.snapshot.paramMap.get('id')];

    this.dialogCreatedSubscription = this.dialogService.dialogCreated.subscribe(dialogId => {
      if (this.activeLink === LinkType.opened){
        this.updateDialogs();
        return;
      }
      const openParam = LinkType[LinkType.opened];
      this.router.navigate([`/dialogs/`, openParam]);
    });
    this.dialogClosedSubscription = this.dialogService.dialogClosed.subscribe(dialogId => {
      this.updateDialogs();
    });

    this.dialogFilterSubscription = this.dialogFilterService.applyAction.subscribe(data => {
      this.dialogCurrentPage = 1;
      this.router.navigate(['/dialogs/'], { queryParams: {...data} });
    });
  }

  updateDialogs(){
    let linkType = this.activeLink;
    const offline = linkType === LinkType.offline;
    if (offline){
      linkType = LinkType.all;
    }

    this.dialogs = of(!Helper.objectIsEmpty(this.dialogFilterData))
        .pipe(switchMap(byQuery => {
                if (!byQuery)
                  return this.dialogService.getDialogs(linkType, this.dialogCurrentPage, this.dialogPageSize, offline);

                return this.dialogService.getDialogsByFilter(this.dialogFilterData, this.dialogCurrentPage, this.dialogPageSize);
              })
            , tap(p => {
                this.dialogCount = p.totalCount;
              })
            , map(p => {
                return p.items
              }));
  }

  ngOnInit(): void {
    this.paramsSubscription = this.activeRoute.params.subscribe(p => {
      // @ts-ignore
      if (!Helper.objectIsEmpty(this.activeRoute.queryParams.value)){
        return;
      }

      let pid: "all" | "opened" | "rejected" | "worked" | "closed" | "offline" = p.id;
      this.activeLink = LinkType[pid];
      this.updateDialogs();
    })
    this.queryParamsSubscription = this.activeRoute.queryParams.subscribe(q => {
      // @ts-ignore
      this.dialogFilterData = {
        ...q
      };
      this.updateDialogs();
    })
    // ?linkType=2&startDate=Wed%20Feb%2017%202021%2000:00:00%20GMT%2B0500&closeDate=Wed%20Feb%2024%202021%2000:00:00%20GMT%2B0500&client=Client&operator=Operator&dialogNumber=12
  }

  ngOnDestroy(): void {
    this.dialogCreatedSubscription.unsubscribe();
    this.paramsSubscription.unsubscribe();
    this.dialogClosedSubscription.unsubscribe();
    this.dialogFilterSubscription.unsubscribe();
    this.queryParamsSubscription.unsubscribe();
  }

  openChat(messageDialog: MessageDialog) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = '600px';
    dialogConfig.height = '700px';
    dialogConfig.panelClass = 'dialog-panel-class'
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

  async close(dialog: MessageDialog) {
    await this.dialogService.close(dialog);
    await this.updateDialogs();
  }

  getPage($event: PageEvent) {
    console.log($event);
    this.dialogCurrentPage = $event.pageIndex + 1;
    this.dialogPageSize = $event.pageSize;
    this.updateDialogs();
  }

  dialogFilterOpen() {
    this.dialogFilterService.open();
  }

  checkStartOrActive(dialogStatus: DialogStatus): boolean {
    return (dialogStatus & (DialogStatus.Started | DialogStatus.Active)) !== 0;
  }
}
