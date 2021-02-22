import {Component, OnDestroy, OnInit} from '@angular/core';
import {animate, state, style, transition, trigger} from "@angular/animations";
import {CommonService} from "../../services/common.service";
import {Subscription} from "rxjs";

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

  constructor(
      private common: CommonService
  ) {
    this.openClosDialogFilterSubscription = this.common.openCloseDialogFilter.subscribe(state => {
      this.opened = state;
    })
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.openClosDialogFilterSubscription.unsubscribe();
  }

  hide() {
    this.common.toggleDialogFilter(false);
  }
}
