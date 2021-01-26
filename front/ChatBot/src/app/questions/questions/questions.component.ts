import { Component, ElementRef, OnInit } from '@angular/core';
import {QuestionsProviderService} from '../../services/questions-provider.service';
import {Observable} from 'rxjs';
import Question from '../../../abstracts/Question';
import {PageDispatcherService} from '../../services/page-dispatcher.service';
import {tap} from 'rxjs/operators';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'questions',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.sass']
})
export class QuestionsComponent implements OnInit {
  questions: Question[] = [];
  private isShowQuestions$: boolean;
  constructor(private questionsProvider: QuestionsProviderService,
              private pageDispatcher: PageDispatcherService
              ) {
  }

  async ngOnInit(): Promise<void> {
    await this.questionsProvider.loadQuestions(undefined);
    this.questionsProvider.existQuestions.pipe(tap(t => {
      console.log(t);
    })).subscribe(res => {
      this.isShowQuestions$ = res;
    });

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

  get isShowQuestions(): boolean{
    console.log(this.isShowQuestions$);
    return this.isShowQuestions$;
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
