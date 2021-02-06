import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {QuestionsProviderService} from '../../services/questions-provider.service';
import {PageHeaderService} from '../../services/page-header.service';
import {CommonService} from '../../services/common.service';
import {Observable} from 'rxjs';

@Component({
  selector: 'questions-header',
  templateUrl: './questions-header.component.html',
  styleUrls: ['./questions-header.component.sass']
})
export class QuestionsHeaderComponent implements OnInit {

  constructor(
    private common: CommonService
  ) { }

  ngOnInit(): void {
  }

  get caption(): Observable<string>{
    return this.common.caption;
  }
}
