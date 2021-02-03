import Page from '../../abstracts/Page';
import {ChatDialogComponent} from '../dialog/chat-dialog.component';
import {QuestionsComponent} from '../questions/questions/questions.component';
import {MainQuestionsComponent} from '../questions/main-questions/main-questions.component';
import {AuthFormComponent} from '../auth-form/auth-form.component';

export class PageSource{
  static getPages(): Page[]{
    return [
      new Page(ChatDialogComponent),
      new Page(QuestionsComponent),
      new Page(MainQuestionsComponent),
      new Page(AuthFormComponent),
    ];
  }
}
