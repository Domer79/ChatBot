import { Component, OnInit } from '@angular/core';
import {QuestionsProviderService} from '../../../services/questions-provider.service';
import {Observable} from 'rxjs';
import Question from '../../../abstracts/Question';

@Component({
  selector: 'questions',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.sass']
})
export class QuestionsComponent implements OnInit {
  questions: Observable<Question[]>;
  constructor(private questionsProvider: QuestionsProviderService) { }

  ngOnInit(): void {
    this.questions = this.questionsProvider.getQuestions();
  }

  showResponse(question: Question): void {
    this.questionsProvider.showQuestionResponse(question);
  }

  get responsePresent(): boolean{
    return this.questionsProvider.checkSelectedQuestion();
  }

  get selectedQuestion(): Question{
    return this.questionsProvider.getSelectedQuestion();
  }
}
