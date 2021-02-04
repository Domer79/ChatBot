import {ChangeDetectionStrategy, Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {QuestionsProviderService} from '../../services/questions-provider.service';
import {Observable, Subscription} from 'rxjs';
import Question from '../../../abstracts/Question';
import {QuestionsComponent} from '../questions/questions.component';
import {PageDispatcherService} from '../../services/page-dispatcher.service';
import {map, tap} from 'rxjs/operators';
import Helper from '../../../misc/Helper';

@Component({
  selector: 'search-questions',
  templateUrl: './search-questions.component.html',
  styleUrls: ['../questions/questions.component.sass'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SearchQuestionsComponent implements OnInit, OnDestroy {
  private searchQuerySubscription: Subscription;

  @Input() searchQuery: string;
  @Output() notFound: EventEmitter<any> = new EventEmitter<any>();

  questions: Observable<Question[]>;
  nothingToShow: boolean;

  constructor(
    private questionsProvider: QuestionsProviderService,
    private pageDispatcher: PageDispatcherService
  ) {
  }

  ngOnInit(): void {
    this.questions = this.questionsProvider.searchQuestions(this.searchQuery).pipe(tap(questions => {
      this.nothingToShow = questions.length === 0;
    }));
    this.searchQuerySubscription = this.questionsProvider.searchQuery.pipe(map(query => {
      return Helper.searchQueryFilter(query);
    })).subscribe(query => {
      this.nothingToShow = false;
      this.questions = this.questionsProvider.searchQuestions(query).pipe(tap(questions => {
          this.nothingToShow = questions.length === 0;
      }));
    });
  }

  showQuestions(question: Question): void {
    this.pageDispatcher.showPage(QuestionsComponent, question);
  }

  ngOnDestroy(): void {
    this.searchQuerySubscription.unsubscribe();
  }
}
