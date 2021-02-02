import {AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {PageDispatcherService} from '../../services/page-dispatcher.service';
import {Observable, Subject, Subscription} from 'rxjs';
import Question from '../../../abstracts/Question';
import {QuestionsProviderService} from '../../services/questions-provider.service';
import {QuestionsComponent} from '../questions/questions.component';
import {MainQuestionsState} from './MainQuestionsState';
import {MainQuestionsComponentBackService} from '../../services/main-questions-component-back.service';
import {MainBackService} from '../../services/main-back.service';

@Component({
  selector: 'app-main-questions',
  templateUrl: './main-questions.component.html',
  styleUrls: ['./main-questions.component.sass']
})
export class MainQuestionsComponent implements OnInit, OnDestroy, AfterViewInit {
  isSearchQueryInput: boolean;
  questions: Observable<Question[]>;
  public data: MainQuestionsState = undefined;

  @ViewChild('searchEditor') searchEditor: ElementRef;

  constructor(
    private pageDispatcher: PageDispatcherService,
    private questionsProvider: QuestionsProviderService,
    private backService: MainQuestionsComponentBackService,
    private mainBackService: MainBackService
  ) {
  }

  ngOnInit(): void {
    this.mainBackService.setComponent(MainQuestionsComponent);
    this.questions = this.questionsProvider.getShortParentQuestions();
    if (!this.data){
      this.data = new MainQuestionsState();
    }
  }

  ngAfterViewInit(): void {
    if (this.data.searchQuery){
      this.searchEditor.nativeElement.innerHTML = this.data.searchQuery;
      this.questionsProvider.setSearchQuery(this.data.searchQuery);
      this.searchQuestion();
    }
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
    this.questionsProvider.setSearchQuery(eventTarget.innerHTML);
    // this.isSearchQueryInput = true; TODO: пока не удалять
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
  }

  private search(): void {
    this.searchQuestion();
  }
}
