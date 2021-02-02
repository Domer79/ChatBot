import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {QuestionsProviderService} from '../../services/questions-provider.service';
import {MainBackService} from '../../services/main-back.service';

@Component({
  selector: 'questions-header',
  templateUrl: './questions-header.component.html',
  styleUrls: ['./questions-header.component.sass']
})
export class QuestionsHeaderComponent implements OnInit {
  @Input() responsePreset: boolean;
  @Output() close: EventEmitter<any> = new EventEmitter<any>();

  constructor(
    private questionsProvider: QuestionsProviderService,
    private mainBackService: MainBackService
  ) { }

  ngOnInit(): void {
  }

  async getBackQuestions(): Promise<void> {
    this.mainBackService.goBack();
    const question = this.questionsProvider.getSelectedQuestion();
    await this.questionsProvider.loadQuestions(question);
  }

  closePage(): void {
    this.close.emit();
  }
}
