import { Component, OnInit } from '@angular/core';
import {QuestionsProviderService} from '../../services/questions-provider.service';
import {Observable} from 'rxjs';
import Question from '../../../abstracts/Question';
import {PageDispatcherService} from '../../services/page-dispatcher.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'questions',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.sass']
})
export class QuestionsComponent implements OnInit {
  questions: Question[] = [];
  constructor(private questionsProvider: QuestionsProviderService,
              private pageDispatcher: PageDispatcherService
              ) {
  }

  async ngOnInit(): Promise<void> {
    await this.questionsProvider.loadQuestions(undefined);
    this.questionsProvider.questions.subscribe(q => {
      this.questions = q;
    });
  }

  async showQuestions(question: Question): Promise<void>{
    await this.questionsProvider.loadQuestions(question);
  }

  showResponse(question: Question): void {
    this.questionsProvider.showQuestionResponse(question);
  }

  get isShowQuestions(): Observable<boolean>{
    return this.questionsProvider.existQuestions;
  }

  get isShowBack(): boolean{
    return this.questionsProvider.isShowBack;
  }

  get selectedQuestion(): Question{
    return this.questionsProvider.getSelectedQuestion();
  }

  closePage(): void {
    this.pageDispatcher.closeCurrent();
  }
}
