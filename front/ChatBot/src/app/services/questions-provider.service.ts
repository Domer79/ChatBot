import { Injectable } from '@angular/core';
import Question from '../../abstracts/Question';
import {AsyncSubject, Observable, of, Subject} from 'rxjs';
import {TokenService} from './token.service';
import {HttpClient} from '@angular/common/http';
import {NIL as guidEmpty, v4 as uuidv4} from 'uuid';

@Injectable({
  providedIn: 'root'
})
export class QuestionsProviderService {
  private questions$: Subject<Question[]> = new Subject<Question[]>();
  private existQuestions$: Subject<boolean> = new Subject<boolean>();
  public questions: Observable<Question[]> = this.questions$.asObservable();
  public existQuestions = this.existQuestions$.asObservable();
  private selectedQuestions: Question[] = [];
  private existQuestionCache: { [key: string]: boolean | undefined } = {};

  constructor(
    private httpClient: HttpClient
  ) { }

  loadQuestions(question: Question | undefined): void{
    if (!question)
    {
      question = new Question();
      question.id = guidEmpty;
    }

    if (this.selectedQuestions.filter(_ => _.id === question.id).length === 0) {
      this.selectedQuestions.push(question);
    }

    if (!this.existQuestionCache.hasOwnProperty(question.id)){
      try {
        this.httpClient.get<boolean>('api/Question/ExistQuestions', {params: {parentId: question.id}}).subscribe(res => {
          this.existQuestionCache[question.id] = res;
          this.existQuestions$.next(res);
        });
      }
      catch (err){
        console.log(err);
      }
    }
    else{
      this.existQuestions$.next(this.existQuestionCache[question.id]);
    }

    this.httpClient.get<Question[]>('api/Question/GetQuestions', { params: { parentId: question.id } }).subscribe(q => {
      this.questions$.next(q);
    });
  }

  showQuestionResponse(question: Question): void {
    this.selectedQuestions.push(question);
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
