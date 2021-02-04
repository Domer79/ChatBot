import {Injectable, Type} from '@angular/core';
import {BackService} from '../../abstracts/BackService';
import {MainQuestionsComponent} from '../questions/main-questions/main-questions.component';
import {Observable, Subject} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MainQuestionsComponentBackService implements BackService {

  constructor() { }

  getComponent(): Type<any> {
    return MainQuestionsComponent;
  }

  goBack(): void {
    return;
  }

  isShowBack(): boolean {
    return false;
  }

  isBackWithClose(): boolean {
    return false;
  }
}
