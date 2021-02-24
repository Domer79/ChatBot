import {Injectable, OnDestroy} from '@angular/core';
import {Observable, Subject, Subscription} from "rxjs";
import {DialogFilterData} from "../../abstracts/dialog-filter-data";
import {ActivatedRoute} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class DialogFilterService implements OnDestroy{
  private openCloseDialogFilter$: Subject<boolean> = new Subject<boolean>();
  private queryParamsSubscription: Subscription;
  private dialogFilterData$: Subject<DialogFilterData | { [key: string]: any; }> = new Subject<DialogFilterData | {[p: string]: any}>();
  private applyAction$: Subject<DialogFilterData> = new Subject<DialogFilterData>();

  openCloseDialogFilter = this.openCloseDialogFilter$.asObservable();
  applyAction = this.applyAction$.asObservable();
  dialogFilterDataFromQueryParams: Observable<DialogFilterData | { [key: string]: any; }> = this.dialogFilterData$.asObservable();

  constructor(
      private activeRoute: ActivatedRoute,
  ) {
    this.queryParamsSubscription = this.activeRoute.queryParams.subscribe(params => {
      this.dialogFilterData$.next({
        ...params
      });
    })
  }

  private toggleDialogFilter(newState: boolean): void{
    this.openCloseDialogFilter$.next(newState);
  }

  open(): void{
    this.toggleDialogFilter(true);
  }

  close(): void{
    this.toggleDialogFilter(false);
  }

  apply(data: DialogFilterData): void{
    this.applyAction$.next(data);
  }

  ngOnDestroy(): void {
    this.queryParamsSubscription.unsubscribe();
  }
}
