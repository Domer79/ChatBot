import {AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {PageDispatcherService} from '../../services/page-dispatcher.service';
import {Observable, Subject, Subscription} from 'rxjs';
import Question from '../../../abstracts/Question';
import {QuestionsProviderService} from '../../services/questions-provider.service';
import {QuestionsComponent} from '../questions/questions.component';
import {MainQuestionsState} from './MainQuestionsState';
import {MainQuestionsComponentBackService} from '../../services/main-questions-component-back.service';
import {PageHeaderService} from '../../services/page-header.service';
import {PageComponent} from '../../decoratotrs/custom-named.decorator';
import Helper from '../../../misc/Helper';
import {ShowChatEditor} from '../../../abstracts/ShowChatEditor';
import {AuthService} from '../../services/auth.service';
import {CommonService} from '../../services/common.service';

@PageComponent({
  customName: 'MainQuestionsComponent',
  iconFile: 'home.svg'
})
@Component({
  selector: 'app-main-questions',
  templateUrl: './main-questions.component.html',
  styleUrls: ['./main-questions.component.sass']
})
export class MainQuestionsComponent implements OnInit, OnDestroy, AfterViewInit, ShowChatEditor {
  private placeholderSubscription: Subscription;

  isSearchQueryInput: boolean;
  questions: Observable<Question[]>;
  public data: MainQuestionsState = undefined;

  @ViewChild('searchEditor') searchEditor: ElementRef;

  constructor(
    private pageDispatcher: PageDispatcherService,
    private questionsProvider: QuestionsProviderService,
    private authService: AuthService,
    private common: CommonService,
  ) {
  }

  ngOnInit(): void {
    this.questions = this.questionsProvider.getShortParentQuestions();
    if (!this.data){
      this.data = new MainQuestionsState();
      this.data.searchQuery = '';
    }
  }

  ngAfterViewInit(): void {
    if (this.data.searchQuery){
      this.searchEditor.nativeElement.innerHTML = this.data.searchQuery;
      this.questionsProvider.setSearchQuery(this.data.searchQuery);
      this.searchQuestion();
    }

    this.placeholderSubscription = this.common.questionSearchPlaceHolder.subscribe(_ => {
      this.searchEditor.nativeElement.setAttribute('placeholder', _);
    });
  }

  closePage(): void {
    this.pageDispatcher.closeCurrent();
  }

  searchQuestionClick(): void {
    this.search();
  }

  searchQuestion(): void {
    console.log(this.data.searchQuery);
    this.questionsProvider.setSearchQuery(this.data.searchQuery);
    this.isSearchQueryInput = true;
  }

  inputSearchQuery(eventTarget: EventTarget | any): void {
    this.data.searchQuery = eventTarget.innerHTML;

    if (Helper.searchQueryFilter(this.data.searchQuery) === ''){
      this.isSearchQueryInput = false;
    }

    this.questionsProvider.setSearchQuery(eventTarget.innerHTML);
  }

  enter($event: KeyboardEvent): void {
    if (this.data.searchQuery === '' && ($event.code === 'Enter' && $event.shiftKey)){
      $event.preventDefault();
    }
  }

  onKeydownEnter($event: Event): void {
    const event = $event as KeyboardEvent;
    if (this.data.searchQuery === ''){
      event.preventDefault();
      return;
    }

    if (!event.shiftKey)
    {
      this.search();
      $event.preventDefault();
    }
  }

  selectQuestion($event: Question): void {
    this.pageDispatcher.showPage(QuestionsComponent, $event);
  }

  ngOnDestroy(): void {
    this.placeholderSubscription.unsubscribe();
  }

  private search(): void {
    this.searchQuestion();
  }

  canShowEditor(): Observable<boolean> {
    return this.authService.isActive();
  }

  get salam1(): Observable<string>{
    return this.common.salam1;
  }
}
