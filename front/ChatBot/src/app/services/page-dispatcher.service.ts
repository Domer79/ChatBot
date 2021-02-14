import {ComponentRef, Injectable, Type} from '@angular/core';
import Page from '../../abstracts/Page';
import {BehaviorSubject, Observable, Subject} from 'rxjs';
import {PageSource} from './PageSource';

@Injectable({
  providedIn: 'root'
})
export class PageDispatcherService {
  private readonly page$: BehaviorSubject<Page>;
  private readonly page: Observable<Page>;
  private readonly pageWithInstance$: Subject<Page>;
  private readonly pageWithInstance: Observable<Page>;
  private readonly closeChat$: Subject<void>;
  private readonly closeChat: Observable<void>;
  private readonly pages: Page[] = PageSource.getPages();
  private readonly stackPage: Page[] = [];

  constructor() {
    this.page$ = new BehaviorSubject<Page>(this.pages[0]);
    this.page = this.page$.asObservable();
    this.pageWithInstance$ = new Subject<Page>();
    this.pageWithInstance = this.pageWithInstance$.asObservable();
    this.closeChat$ = new Subject<void>();
    this.closeChat = this.closeChat$.asObservable();
    this.stackPage.push(this.pages[0]);
    this.update();
  }

  getCurrent(): Page{
    return this.stackPage[this.stackPage.length - 1];
  }

  showPage(component: Type<any>, data: any = null): void{
    const page = this.pages.filter(_ => _.component === component)[0];
    page.data = data;
    this.stackPage.push(page);
    this.update();
  }

  closeCurrent(): void{
    if (this.stackPage.length === 1){
      this.closeChat$.next();
      return;
    }

    this.stackPage.pop();
    this.update();
  }

  getPage(): Observable<Page>{
    return this.page;
  }

  getPageWithInstance(): Observable<Page>{
    return this.pageWithInstance;
  }

  getCloseChatEvent(): Observable<void>{
    return this.closeChat;
  }

  setCurPageInstance(instance: ComponentRef<any>): void{
    this.getCurrent().instance = instance;
    this.updateWithInstance();
  }

  closeChatFull(): void{
    this.closeChat$.next();
  }

  private update(): void{
    this.page$.next(this.getCurrent());
  }

  private updateWithInstance(): void {
    this.pageWithInstance$.next(this.getCurrent());
  }
}
