import {Component, ElementRef, OnDestroy, OnInit} from '@angular/core';
import {QuestionsProviderService} from '../../services/questions-provider.service';
import {Observable, Subscription} from 'rxjs';
import Question from '../../../abstracts/Question';
import {PageDispatcherService} from '../../services/page-dispatcher.service';
import {tap} from 'rxjs/operators';
import {PageHeaderService} from '../../services/page-header.service';
import {CustomNamed} from '../../decoratotrs/custom-named.decorator';
import {QuestionComponentBackService} from '../../services/question-component-back.service';
import {BackService, HasBackService} from '../../../abstracts/BackService';
import {ShowChatEditor} from '../../../abstracts/ShowChatEditor';
import {AuthService} from '../../services/auth.service';

@CustomNamed('QuestionsComponent')
@Component({
  // tslint:disable-next-line:component-selector
  selector: 'questions',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.sass'],
})
export class QuestionsComponent implements OnInit, OnDestroy, HasBackService, ShowChatEditor {
  questions: Question[] = [];
  private isShowQuestions$: boolean;
  private existQuestionSubscription: Subscription;
  constructor(private questionsProvider: QuestionsProviderService,
              private pageDispatcher: PageDispatcherService,
              private backService: QuestionComponentBackService,
              private authService: AuthService
              ) {
    this.existQuestionSubscription = this.questionsProvider.existQuestions.subscribe(res => {
      this.isShowQuestions$ = res;
    });
  }

  public data: Question;

  async ngOnInit(): Promise<void> {
    await this.questionsProvider.loadQuestions(this.data ?? undefined);

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
    return this.isShowQuestions$;
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

  getBackService(): BackService {
    return this.backService;
  }

  canShowEditor(): Observable<boolean> {
    return this.authService.isActive();
  }
}
