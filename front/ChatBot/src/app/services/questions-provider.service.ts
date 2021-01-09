import { Injectable } from '@angular/core';
import Question from '../../abstracts/Question';
import {Observable, of, Subject} from 'rxjs';
import {TokenService} from './token.service';
import {HttpClient} from '@angular/common/http';
import {NIL as guidEmpty, v4 as uuidv4} from 'uuid';

@Injectable({
  providedIn: 'root'
})
export class QuestionsProviderService {
  private questions$: Subject<Question[]> = new Subject<Question[]>();
  public questions: Observable<Question[]> = this.questions$.asObservable();
  private selectedQuestions: Question[] = [];
  public endQuestions: boolean;

  constructor(
    private httpClient: HttpClient
  ) { }

  async loadQuestions(question: Question | undefined): Promise<void>{
    if (!question)
    {
      question = new Question();
      question.id = guidEmpty;
    }

    if (this.selectedQuestions.filter(_ => _.id === question.id).length === 0) {
      this.selectedQuestions.push(question);
    }

    this.endQuestions = !(await this.httpClient
      .get<boolean>('api/Question/ExistQuestions', {params: {parentId: question.id}})
      .toPromise());

    this.httpClient.get<Question[]>('api/Question/GetQuestions', { params: { parentId: question.id } }).subscribe(q => {
      this.questions$.next(q);
    });
  }

  showQuestionResponse(question: Question): void {
    this.selectedQuestions.push(question);
  }

  get isShowResponse(): boolean{
    return this.endQuestions;
  }

  get isShowBack(): boolean{
    return this.selectedQuestions.length > 1;
  }

  getBackQuestions(): Question | undefined{
    if (this.selectedQuestions.length === 0)
    {
      throw new Error('Can\'t stop showing because there\'s nothing to show');
    }

    this.selectedQuestions.pop();
    return this.selectedQuestions[this.selectedQuestions.length - 1];
  }

  getSelectedQuestion(): Question{
    if (this.selectedQuestions.length === 0)
    {
      throw new Error('Can\'t stop showing because there\'s nothing to show');
    }

    return this.selectedQuestions[this.selectedQuestions.length - 1];
  }
}
