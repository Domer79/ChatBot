import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {PageHeaderService} from '../../services/page-header.service';
import {CommonService} from '../../services/common.service';
import {Observable} from 'rxjs';

@Component({
  selector: 'main-questions-header',
  templateUrl: './main-questions-header.component.html',
  styleUrls: ['./main-questions-header.component.sass']
})
export class MainQuestionsHeaderComponent implements OnInit {
  @Output() close: EventEmitter<any> = new EventEmitter<any>();

  caption: Observable<string>;

  constructor(
    private common: CommonService
  ) {
    this.caption = this.common.caption;
  }

  ngOnInit(): void {
  }
}
