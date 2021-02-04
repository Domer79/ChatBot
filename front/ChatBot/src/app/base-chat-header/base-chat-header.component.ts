import { Component, OnInit } from '@angular/core';
import {PageHeaderService} from '../services/page-header.service';

@Component({
  selector: 'base-chat-header',
  templateUrl: './base-chat-header.component.html',
  styleUrls: ['./base-chat-header.component.sass']
})
export class BaseChatHeaderComponent implements OnInit {

  constructor(
    private headerService: PageHeaderService
  ) { }

  ngOnInit(): void {
  }

  get hasBack(): boolean{
    return this.headerService.hasBack();
  }

  goBack(): void{
    this.headerService.goBack();
  }

  closePage(): void{
    this.headerService.closePage();
  }
}
