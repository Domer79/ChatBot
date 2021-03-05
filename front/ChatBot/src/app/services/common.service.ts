import {HostListener, Injectable, OnInit} from '@angular/core';
import {Time} from '@angular/common';
import {NumberSettings, Settings, Shift} from '../../abstracts/settings';
import {forkJoin, Observable, ReplaySubject, Subject, Subscription, timer, zip} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {map, repeatWhen, switchAll, switchMap, take, takeUntil, tap, zipAll} from 'rxjs/operators';
import Helper from '../../misc/Helper';

@Injectable({
  providedIn: 'root'
})
export class CommonService{
  private beginShift$: ReplaySubject<NumberSettings> = new ReplaySubject<NumberSettings>(1);
  private closeShift$: ReplaySubject<NumberSettings> = new ReplaySubject<NumberSettings>(1);
  private clientTimeoutInterval$: ReplaySubject<NumberSettings> = new ReplaySubject<NumberSettings>(1);
  private salam1$: ReplaySubject<string> = new ReplaySubject<string>(1);
  private salam2$: ReplaySubject<string> = new ReplaySubject<string>(1);
  private sendedMessage$: ReplaySubject<string> = new ReplaySubject<string>(1);
  private captionMessage$: ReplaySubject<string> = new ReplaySubject<string>(1);
  private questionSearchPlaceHolder$: ReplaySubject<string> = new ReplaySubject<string>(1);
  private closeSessionByTimeout$: Subject<void> = new Subject<void>();
  private beginShift: Observable<NumberSettings> = this.beginShift$.asObservable();
  private closeShift: Observable<NumberSettings> = this.closeShift$.asObservable();
  private stopTimeout$: Subject<void> = new Subject<void>();
  private startTimeout$: Subject<void> = new Subject<void>();
  private closeSessionSubscription: Subscription;

  salam1: Observable<string> = this.salam1$.asObservable();
  salam2: Observable<string> = this.salam2$.asObservable();
  sendedMessage: Observable<string> = this.sendedMessage$.asObservable();
  caption: Observable<string> = this.captionMessage$.asObservable();
  questionSearchPlaceHolder: Observable<string> = this.questionSearchPlaceHolder$.asObservable();
  closeSessionByTimeout: Observable<any> = this.closeSessionByTimeout$.asObservable();

  constructor(
    private httpClient: HttpClient,
  ) {
    this.httpClient.get<Settings[]>('api/Settings/GetSettings').pipe(tap(allSettings => {
      for (const settings of allSettings){
        const numberSettings = {
          id: settings.id,
          name: settings.name,
          description: settings.description,
          value: settings.value,
          dateCreated: settings.dateCreated,
          numberValue: Number.parseFloat(settings.value)
        };
        switch (settings.name) {
          case 'beginShift': {
            this.beginShift$.next(numberSettings);
            break;
          }
          case 'closeShift': {
            this.closeShift$.next(numberSettings);
            break;
          }
          case 'salam1': {
            this.salam1$.next(settings.value);
            break;
          }
          case 'salam2': {
            this.salam2$.next(settings.value);
            break;
          }
          case 'sendedMessage': {
            this.sendedMessage$.next(settings.value);
            break;
          }
          case 'caption': {
            this.captionMessage$.next(settings.value);
            break;
          }
          case 'questionSearchPlaceHolder': {
            this.questionSearchPlaceHolder$.next(settings.value);
            break;
          }
          case 'clientTimeoutInterval': {
            this.clientTimeoutInterval$.next(numberSettings);
            break;
          }
        }
      }
    })).subscribe();
  }

  public getShift(): Observable<Shift>{
    return zip(this.beginShift, this.closeShift).pipe(map(val => {
      const closingShift = val.filter(_ => _.name === 'closeShift')[0];
      const beginShift = val.filter(_ => _.name === 'beginShift')[0];
      return { begin: beginShift.numberValue, close: closingShift.numberValue };
    }));
  }

  public serverTimeoutInMilliseconds(): Observable<number>{
    return this.clientTimeoutInterval$.pipe(map(val => {
      return val.numberValue * 1000 * 60;
      // return 0.3 * 1000 * 60;
    }));
  }

  public runMessageTimeout(): void{
    if (!this.closeSessionSubscription) {
      this.closeSessionSubscription = this.serverTimeoutInMilliseconds()
        .pipe(
          switchMap(serverTimeout => {
            return timer(serverTimeout, serverTimeout).pipe(
              takeUntil(this.stopTimeout$),
              repeatWhen(() => this.startTimeout$)
            );
          })
        )
        .subscribe(() => {
          this.closeSessionByTimeout$.next();
          this.stopTimeout();
          this.closeSessionSubscription.unsubscribe();
        });
    }
  }

  stopTimeout(): void{
    if (this.closeSessionSubscription){
      this.stopTimeout$.next();
    }
  }

  startTimeout(): void{
    this.runMessageTimeout();
    this.startTimeout$.next();
  }

  restartTimeout(): void{
    this.stopTimeout();
    this.startTimeout();
  }
}
