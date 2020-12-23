import Page from '../../abstracts/Page';
import {ChatDialogComponent} from '../dialog/chat-dialog.component';
import {QuestionsComponent} from '../questions/questions/questions.component';

export class PageSource{
  static getPages(): Page[]{
    return [
      new Page(ChatDialogComponent),
      new Page(QuestionsComponent)
    ];
  }
}
