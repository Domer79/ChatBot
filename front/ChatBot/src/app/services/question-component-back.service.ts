import {Injectable, Type} from '@angular/core';
import {BackService} from '../../abstracts/BackService';
import {QuestionsProviderService} from './questions-provider.service';
import {QuestionsComponent} from '../questions/questions/questions.component';

@Injectable({
  providedIn: 'root'
})
export class QuestionComponentBackService implements BackService{

  constructor(
    private questionProvider: QuestionsProviderService
  ) { }

  goBack(): void {
    this.questionProvider.goBack();
  }

  isShowBack(): boolean {
    return this.questionProvider.isShowBack;
  }

  getComponent(): Type<any> {
    return QuestionsComponent;
  }
}
