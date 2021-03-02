import {Component, OnInit, Output, EventEmitter} from '@angular/core';
import {Observable} from 'rxjs';
import {CommonService} from '../services/common.service';

@Component({
  selector: 'chat-header',
  templateUrl: './chat-header.component.html',
  styleUrls: ['./chat-header.component.sass']
})
export class ChatHeaderComponent implements OnInit {
  @Output() openQuestions = new EventEmitter<void>();
  caption: Observable<string>;

  constructor(
    private common: CommonService
  ) {
    this.caption = this.common.caption;
  }

  ngOnInit(): void {
  }
}
