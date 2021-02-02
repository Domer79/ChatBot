import {ChangeDetectionStrategy, Component, Input, OnDestroy, OnInit} from '@angular/core';
import {QuestionsProviderService} from '../../services/questions-provider.service';
import {Observable, Subscription} from 'rxjs';
import Question from '../../../abstracts/Question';
import {QuestionsComponent} from '../questions/questions.component';
import {PageDispatcherService} from '../../services/page-dispatcher.service';

@Component({
  selector: 'search-questions',
  templateUrl: './search-questions.component.html',
  styleUrls: ['../questions/questions.component.sass'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SearchQuestionsComponent implements OnInit, OnDestroy {
  @Input() searchQuery: string;
  questions: Observable<Question[]>;
  private searchQuerySubscription: Subscription;

  constructor(
    private questionsProvider: QuestionsProviderService,
    private pageDispatcher: PageDispatcherService
  ) {
  }

  ngOnInit(): void {
    this.questions = this.questionsProvider.searchQuestions(this.searchQuery);
    this.searchQuerySubscription = this.questionsProvider.searchQuery.subscribe(query => {
      this.questions = this.questionsProvider.searchQuestions(query);
    });
  }

  showQuestions(question: Question): void {
    this.pageDispatcher.showPage(QuestionsComponent, question);
  }

  ngOnDestroy(): void {
    this.searchQuerySubscription.unsubscribe();
  }
}
