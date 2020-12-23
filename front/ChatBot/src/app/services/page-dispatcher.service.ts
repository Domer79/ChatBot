import {Injectable, Type} from '@angular/core';
import Page from '../../abstracts/Page';
import {BehaviorSubject, Observable, Subject} from 'rxjs';
import {PageSource} from './PageSource';

@Injectable({
  providedIn: 'root'
})
export class PageDispatcherService {
  private pages$: BehaviorSubject<Page>;
  private pages: Page[] = PageSource.getPages();

  private stackPage: Page[] = [];

  constructor() {
    this.pages$ = new BehaviorSubject<Page>(this.pages[0]);
    this.stackPage.push(this.pages[0]);
    this.update();
  }

  getCurrent(): Page{
    return this.stackPage[this.stackPage.length - 1];
  }

  showPage(component: Type<any>): void{
    const page = this.pages.filter(_ => _.component === component)[0];
    this.stackPage.push(page);
    this.update();
  }

  closeCurrent(): void{
    if (this.stackPage.length === 1) {
      throw new Error('The dialogue cannot be closed');
    }

    this.stackPage.pop();
    this.update();
  }

  private update(): void{
    this.pages$.next(this.getCurrent());
  }

  getPage(): Observable<Page>{
    return this.pages$.asObservable();
  }
}
