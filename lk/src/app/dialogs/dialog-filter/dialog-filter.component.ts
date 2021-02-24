import {Component, OnDestroy, OnInit} from '@angular/core';
import {animate, state, style, transition, trigger} from "@angular/animations";
import {DialogFilterService} from "../../services/dialog-filter.service";
import {Subscription} from "rxjs";
import {DialogStatus, LinkType} from "../../contracts/message-dialog";
import {DialogFilterData} from "../../../abstracts/dialog-filter-data";
import Helper from "../../misc/Helper";

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
  // @ts-ignore
  selectedLinkType: { value: LinkType, description: string } = {};
  linkTypes: { value: LinkType, description: string }[];
  dialogFilterData: DialogFilterData | any = {};

  constructor(
      private dialogFilter: DialogFilterService
  ) {
    this.linkTypes = Helper.getLinkTypeDescriptions();
    // TODO: add subscription
    dialogFilter.dialogFilterData.subscribe(data => {
      debugger;
      if (data.linkType){
        this.selectedLinkType = Helper.getLinkTypeDescription(data.linkType);
      }

      // @ts-ignore
      this.dialogFilterData = {
        ...data
      };
    });

    this.openClosDialogFilterSubscription = this.dialogFilter.openCloseDialogFilter.subscribe(state => {
      this.opened = state;
    })
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
