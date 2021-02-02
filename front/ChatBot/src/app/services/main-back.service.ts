import {Injectable, Type} from '@angular/core';
import {BackService} from '../../abstracts/BackService';
import {QuestionComponentBackService} from './question-component-back.service';
import {MainQuestionsComponentBackService} from './main-questions-component-back.service';
import {QuestionsComponent} from '../questions/questions/questions.component';
import {MainQuestionsComponent} from '../questions/main-questions/main-questions.component';
import {PageDispatcherService} from './page-dispatcher.service';
import {last} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MainBackService implements BackService {
  private components: Type<any>[] = [];
  private serviceDictionary: { [key: string]: BackService } = {};

  constructor(
    private questionComponentBackService: QuestionComponentBackService,
    private mainQuestionComponentBackService: MainQuestionsComponentBackService,
    private pageDispatcher: PageDispatcherService
  ) {
    this.serviceDictionary[QuestionsComponent.name] = questionComponentBackService;
    this.serviceDictionary[MainQuestionsComponent.name] = mainQuestionComponentBackService;
  }

  getComponent(): Type<any> {
    return undefined;
  }

  goBack(): void {
    if (!this.isShowBack()){
      throw new Error('It is not possible to go back');
    }

    const lastComponent = this.components.pop();
    const current = this.current;
    if (current.name !== lastComponent.name){
      this.pageDispatcher.showPage(this.current);
      this.serviceDictionary[this.current.name].goBack();
    }
    else if (this.serviceDictionary[this.current.name].isShowBack()){
      this.serviceDictionary[this.current.name].goBack();
    }
  }

  isShowBack(): boolean {
    return this.components.length > 1;
  }

  setComponent(component: Type<any>): void{
    if (component.name === MainQuestionsComponent.name){
      if (this.components.filter(_ => _.name === MainQuestionsComponent.name).length > 0){
        return;
      }
    }
    this.components.push(component);
  }

  private get current(): Type<any>{
    return this.components[this.components.length - 1];
  }

}
