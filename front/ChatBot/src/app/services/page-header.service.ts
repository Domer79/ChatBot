import {Injectable, OnDestroy, Type} from '@angular/core';
import {PageDispatcherService} from './page-dispatcher.service';
import Page from '../../abstracts/Page';
import {Subscription} from 'rxjs';
import {BackService, HasBackService} from '../../abstracts/BackService';

@Injectable({
  providedIn: 'root'
})
export class PageHeaderService implements OnDestroy {
  currentPage: Page;
  private pageSubscription: Subscription;
  constructor(
    private pageDispatcher: PageDispatcherService
  ) {
    this.pageSubscription = pageDispatcher.getPageWithInstance().subscribe(page => {
      this.currentPage = page;
    });
  }

  ngOnDestroy(): void {
    this.pageSubscription.unsubscribe();
  }

  hasBack(): boolean{
    return this.currentPage.instance
      && (this.currentPage.instance['getBackService'] && typeof(this.currentPage.instance['getBackService']) === 'function')
      && (this.currentPage.instance as unknown as HasBackService).getBackService().isShowBack();
  }

  goBack(): void{
    if (!this.hasBack()){
      throw new Error('The BackService not initialized');
    }

    const backService = (this.currentPage.instance as unknown as HasBackService).getBackService();

    if (backService.isBackWithClose()){
      this.closePage();
    }

    backService.goBack();
  }

  closePage(): void{
    this.pageDispatcher.closeCurrent();
  }
}
