import { Component, OnInit } from '@angular/core';
import {PageHeaderService} from '../services/page-header.service';
import {PageDispatcherService} from '../services/page-dispatcher.service';
import {Observable} from 'rxjs';
import Page from '../../abstracts/Page';
import {map, tap} from 'rxjs/operators';

@Component({
  selector: 'base-chat-header',
  templateUrl: './base-chat-header.component.html',
  styleUrls: ['./base-chat-header.component.sass']
})
export class BaseChatHeaderComponent implements OnInit {
  activePages: Observable<Page[]>;

  constructor(
    private headerService: PageHeaderService,
    private pageDispatcher: PageDispatcherService
  ) {
    this.activePages = this.pageDispatcher.stackPageChange.pipe(map(pages => {
      return pages.filter(_ => _.iconFile !== null);
    }), tap(pages => {
      console.log(pages);
    }));
  }

  ngOnInit(): void {
  }

  get hasBack(): boolean{
    const result = this.headerService.hasBack();
    return result;
  }

  goBack(): void{
    this.headerService.goBack();
  }

  closeChat(): void{
    this.headerService.closeChat();
  }
}
