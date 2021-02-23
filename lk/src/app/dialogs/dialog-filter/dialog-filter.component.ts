import {Component, OnDestroy, OnInit} from '@angular/core';
import {animate, state, style, transition, trigger} from "@angular/animations";
import {DialogFilterService} from "../../services/dialog-filter.service";
import {Subscription} from "rxjs";
import {DialogStatus} from "../../contracts/message-dialog";
import {DialogFilterData} from "../../../abstracts/dialog-filter-data";

@Component({
  selector: 'dialog-filter',
  templateUrl: './dialog-filter.component.html',
  styleUrls: ['./dialog-filter.component.sass'],
  animations: [
    trigger('openCloseFilter', [
      state('open', style({
        opacity: 1,
      })),
      state('closed', style({
        opacity: 0,
      })),
      transition('open => closed', [
        animate('0s')
      ]),
      transition('closed => open', [
        animate('0s')
      ]),
    ])
  ]
})
export class DialogFilterComponent implements OnInit, OnDestroy {
  opened = false;
  private openClosDialogFilterSubscription: Subscription;
  dialogStatuses: { value: DialogStatus, name: string }[];
  dialogStatusType: DialogStatus | any;
  dialogFilterData: DialogFilterData | { [key: string]: any; };

  constructor(
      private dialogFilter: DialogFilterService
  ) {
    // TODO: add subscription
    dialogFilter.dialogFilterData.subscribe(data => {
      this.dialogFilterData = data;
    });
    this.openClosDialogFilterSubscription = this.dialogFilter.openCloseDialogFilter.subscribe(state => {
      this.opened = state;
    })

    for(let name in this.dialogStatusType){
      this.dialogStatuses.push({ value: this.dialogStatusType[name], name })
    }
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.openClosDialogFilterSubscription.unsubscribe();
  }

  hide() {
    this.dialogFilter.toggleDialogFilter(false);
    // this.common.dialogFilterData = {
    //   client
    // };
  }
}
