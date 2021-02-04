import {Injectable, OnDestroy} from '@angular/core';
import {PageDispatcherService} from './page-dispatcher.service';
import {Observable, of, Subscription} from 'rxjs';
import Page from '../../abstracts/Page';
import {HasBackService} from '../../abstracts/BackService';
import {ShowChatEditor} from '../../abstracts/ShowChatEditor';
import {ClientMsgDispatcher} from './client-msg-dispatcher.service';
import Message from '../../abstracts/message';

@Injectable({
  providedIn: 'root'
})
export class ChatEditorService implements OnDestroy{
  private readonly pageSubscription: Subscription;
  private readonly messageSubscription: Subscription;
  private currentPage: Page;
  private messages: Message[] = [];

  constructor(
    private pageDispatcher: PageDispatcherService,
    private clientMsgDispatcher: ClientMsgDispatcher
  ) {
    this.pageSubscription = pageDispatcher.getPageWithInstance().subscribe(page => {
      this.currentPage = page;
    });

    this.messageSubscription = this.clientMsgDispatcher.messages.subscribe(message => {
      this.messages.push(message);
      this.messageSubscription.unsubscribe();
    });
  }

  private isImplementShowChatEditor(): boolean{
    return this.currentPage && this.currentPage.instance
      && (this.currentPage.instance['canShowEditor'] && typeof(this.currentPage.instance['canShowEditor']) === 'function');
  }

  canShow(): Observable<boolean>{
    if (!this.isImplementShowChatEditor()){
      return of(false);
    }

    if (this.messages.length === 0){
      return of(true);
    }

    const shower = this.currentPage.instance as unknown as ShowChatEditor;
    return shower.canShowEditor();
  }

  ngOnDestroy(): void {
    this.pageSubscription.unsubscribe();

    if (this.messageSubscription){
      this.messageSubscription.unsubscribe();
    }
  }
}
