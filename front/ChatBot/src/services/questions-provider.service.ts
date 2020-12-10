import { Injectable } from '@angular/core';
import Question from '../abstracts/Question';
import {Observable, of} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class QuestionsProviderService {
  private selectedQuestions: Question[] = [];

  private questions: Question[] = [
    { id: '1', question: 'Вопрос1', response: 'Ответ 1' },
    { id: '3', question: 'Вопрос2', response: 'Ответ 2' },
    { id: '4', question: 'Вопрос3', response: 'Ответ 3' },
    { id: '2', question: 'Вопрос4', response: 'Ответ 4' },
    { id: '5', question: 'Вопрос5', response: 'Ответ 5' },
    { id: '6', question: 'Вопрос6', response: 'Ответ 6' }
  ];

  constructor() { }

  getQuestions(): Observable<Question[]>{
    return of(this.questions);
  }

  showQuestionResponse(question: Question): void {
    this.selectedQuestions.push(question);
  }

  stopShowResponse(): void{
    if (this.selectedQuestions.length === 0)
    {
      throw new Error('Can\'t stop showing because there\'s nothing to show');
    }

    this.selectedQuestions.pop();
  }

  checkSelectedQuestion(): boolean {
    return this.selectedQuestions.length !== 0;
  }

  getSelectedQuestion(): Question{
    if (this.selectedQuestions.length === 0)
    {
      throw new Error('Can\'t stop showing because there\'s nothing to show');
    }

    return this.selectedQuestions[this.selectedQuestions.length - 1];
  }
}
