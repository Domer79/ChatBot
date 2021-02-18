import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import Question from "../../abstracts/Question";
import {MatSnackBar} from "@angular/material/snack-bar";

@Injectable({
  providedIn: 'root'
})
export class QuestionService {

  constructor(
      private httpClient: HttpClient,
      private _matSnackBar: MatSnackBar,
  ) { }

  getQuestions(parentId: string): Observable<Question[]>{
    return this.httpClient.get<Question[]>('api/Question/GetQuestions', { params: {parentId} });
  }

  async saveQuestion(question: Question): Promise<void>{
    try {
      if (!question) {
        throw new Error('Argument null exception. Parameter name "question"');
      }

      await this.httpClient.post('api/Question/save', question).toPromise();
      this._matSnackBar.open(`Вопрос сохранен`, 'Закрыть', { duration: 5000 });
    }
    catch (err){
      this._matSnackBar.open(`Не удалось сохранить вопрос`, 'Закрыть', { duration: 5000 });
    }
  }

  async deleteQuestion(question: Question): Promise<void>{
    try {
      if (!question) {
        throw new Error('Argument null exception. Parameter name "question"');
      }

      await this.httpClient.delete('api/Question/delete', {params: {questionId: question.id}}).toPromise();
      this._matSnackBar.open(`Вопрос удален`, 'Закрыть', { duration: 5000 });
    }
    catch (err){
      this._matSnackBar.open(`Не удалось удалить вопрос. ${err.message}`, 'Закрыть', { duration: 5000 });
      throw err;
    }
  }
}
