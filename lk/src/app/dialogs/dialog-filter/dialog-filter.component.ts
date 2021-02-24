import {Component, OnDestroy, OnInit} from '@angular/core';
import {animate, state, style, transition, trigger} from "@angular/animations";
import {DialogFilterService} from "../../services/dialog-filter.service";
import {Subscription} from "rxjs";
import {DialogStatus, LinkType} from "../../contracts/message-dialog";
import {DialogFilterData} from "../../../abstracts/dialog-filter-data";
import Helper from "../../misc/Helper";
import * as _moment from 'moment';
// tslint:disable-next-line:no-duplicate-imports
// @ts-ignore
import {default as _rollupMoment} from 'moment';

const moment = _rollupMoment || _moment;

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
  private openClosDialogFilterSubscription: Subscription;
  private dataSubscription: Subscription;
  selectedLinkType: { value: LinkType, description: string };
  // @ts-ignore
  dialogFilterData: DialogFilterData = {};
  opened = false;

  linkTypes: { value: LinkType, description: string }[];

  constructor(
      private dialogFilter: DialogFilterService
  ) {
    this.linkTypes = Helper.getLinkTypeDescriptions();
    this.dataSubscription = dialogFilter.dialogFilterDataFromQueryParams.subscribe(data => {
      if (data.linkType){
        this.selectedLinkType = Helper.getLinkTypeDescription(data.linkType);
      }

      this.dialogFilterData = {
        linkType: this.selectedLinkType?.value,
        startDate: moment(new Date(data.startDate)),
        closeDate: moment(new Date(data.closeDate)),
        client: data.client,
        operator: data.operator,
        dialogNumber: data.dialogNumber,
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
    this.dataSubscription.unsubscribe();
  }

  hide() {
    this.dialogFilter.close();
  }

  apply() {
    this.hide();
    this.dialogFilterData.linkType = this.selectedLinkType?.value;
    this.dialogFilter.apply(this.dialogFilterData);
  }
}
