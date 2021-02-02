import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {MainBackService} from '../../services/main-back.service';

@Component({
  selector: 'main-questions-header',
  templateUrl: './main-questions-header.component.html',
  styleUrls: ['./main-questions-header.component.sass']
})
export class MainQuestionsHeaderComponent implements OnInit {
  @Output() close: EventEmitter<any> = new EventEmitter<any>();

  constructor(
    private mainBackService: MainBackService
  ) { }

  ngOnInit(): void {
  }

  closePage(): void {
    this.close.emit();
  }

  isShowBack(): boolean {
    return this.mainBackService.isShowBack();
  }

  getBackQuestions(): void {
    return this.mainBackService.goBack();
  }
}
