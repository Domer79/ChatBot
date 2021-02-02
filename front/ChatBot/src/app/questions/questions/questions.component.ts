import {Component, ElementRef, OnDestroy, OnInit} from '@angular/core';
import {QuestionsProviderService} from '../../services/questions-provider.service';
import {Observable, Subscription} from 'rxjs';
import Question from '../../../abstracts/Question';
import {PageDispatcherService} from '../../services/page-dispatcher.service';
import {tap} from 'rxjs/operators';
import {MainBackService} from '../../services/main-back.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'questions',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.sass']
})
export class QuestionsComponent implements OnInit, OnDestroy {
  questions: Question[] = [];
  private isShowQuestions$: boolean;
  private existQuestionSubscription: Subscription;
  constructor(private questionsProvider: QuestionsProviderService,
              private pageDispatcher: PageDispatcherService,
              private mainBackService: MainBackService
              ) {
    this.existQuestionSubscription = this.questionsProvider.existQuestions.subscribe(res => {
      this.isShowQuestions$ = res;
    });
  }

  public data: Question;

  async ngOnInit(): Promise<void> {
    this.mainBackService.setComponent(QuestionsComponent);
    await this.questionsProvider.loadQuestions(this.data ?? undefined);

    this.questionsProvider.questions.subscribe(q => {
      this.questions = q;
    });
  }

  async showQuestions(question: Question): Promise<void>{
    await this.questionsProvider.loadQuestions(question);
    this.mainBackService.setComponent(QuestionsComponent);
  }

  showResponse(question: Question): void {
    this.questionsProvider.showQuestionResponse(question);
  }

  get isShowQuestions(): boolean{
    return this.isShowQuestions$;
  }

  get isShowBack(): boolean{
    return this.mainBackService.isShowBack();
  }

  get selectedQuestion(): Question{
    return this.questionsProvider.getSelectedQuestion();
  }

  closePage(): void {
    this.pageDispatcher.closeCurrent();
  }

  ngOnDestroy(): void {
    this.existQuestionSubscription.unsubscribe();
  }
}
