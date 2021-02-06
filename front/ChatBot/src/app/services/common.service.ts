import {Injectable, OnInit} from '@angular/core';
import {Time} from '@angular/common';
import {NumberSettings, Settings, Shift} from '../../abstracts/settings';
import {Observable, ReplaySubject, zip} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {map, tap} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CommonService{
  private beginShift$: ReplaySubject<NumberSettings> = new ReplaySubject<NumberSettings>(1);
  private closeShift$: ReplaySubject<NumberSettings> = new ReplaySubject<NumberSettings>(1);
  private salam1$: ReplaySubject<string> = new ReplaySubject<string>(1);
  private salam2$: ReplaySubject<string> = new ReplaySubject<string>(1);
  private sendedMessage$: ReplaySubject<string> = new ReplaySubject<string>(1);
  private captionMessage$: ReplaySubject<string> = new ReplaySubject<string>(1);

  private beginShift: Observable<NumberSettings> = this.beginShift$.asObservable();
  private closeShift: Observable<NumberSettings> = this.closeShift$.asObservable();

  salam1: Observable<string> = this.salam1$.asObservable();
  salam2: Observable<string> = this.salam2$.asObservable();
  sendedMessage: Observable<string> = this.sendedMessage$.asObservable();
  caption: Observable<string> = this.captionMessage$.asObservable();

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
          numberValue: Number.parseInt(settings.value, 10)
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
}
