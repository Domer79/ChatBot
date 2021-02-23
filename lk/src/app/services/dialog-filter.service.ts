import {Injectable, OnDestroy} from '@angular/core';
import {Observable, Subject, Subscription} from "rxjs";
import {DialogFilterData} from "../../abstracts/dialog-filter-data";
import {ActivatedRoute} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class DialogFilterService implements OnDestroy{
  private $opeCloseDialogFilter: Subject<boolean> = new Subject<boolean>();
  private queryParamsSubscription: Subscription;
  private dialogFilterData$: Subject<DialogFilterData | { [key: string]: any; }> = new Subject<DialogFilterData | {[p: string]: any}>();

  openCloseDialogFilter = this.$opeCloseDialogFilter.asObservable();
  dialogFilterData: Observable<DialogFilterData | { [key: string]: any; }> = this.dialogFilterData$.asObservable();

  constructor(
      private activeRoute: ActivatedRoute,
  ) {
    this.queryParamsSubscription = this.activeRoute.queryParams.subscribe(params => {
      debugger;
      this.dialogFilterData$.next({
        ...params
      });
    })
  }

  toggleDialogFilter(newState: boolean): void{
    this.$opeCloseDialogFilter.next(newState);
  }

  ngOnDestroy(): void {
    this.queryParamsSubscription.unsubscribe();
  }
}
