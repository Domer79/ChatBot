import {AfterViewInit, ChangeDetectionStrategy, Component, Input} from '@angular/core';
import Message from '../../abstracts/message';
import {MessageType} from '../../misc/message-type';
import Question from '../../abstracts/Question';
import {PageDispatcherService} from '../services/page-dispatcher.service';
import {QuestionsComponent} from '../questions/questions/questions.component';
import {AuthFormComponent} from '../auth-form/auth-form.component';

@Component({
  selector: 'operator-message',
  templateUrl: './operator-message.component.html',
  styleUrls: ['./operator-message.component.sass'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OperatorMessageComponent implements AfterViewInit {
  private question: Question;
  @Input() message: Message;

  messageType = MessageType;

  constructor(
    private pageDispatcher: PageDispatcherService
  ) { }

  ngAfterViewInit(): void {
    if (this.message.type === MessageType.Question){
      this.question = new Question();
      this.question.id = this.message.questionId;
      this.question.question = this.message.question;
    }
  }

  showQuestion(): void {
    this.pageDispatcher.showPage(QuestionsComponent, this.question);
  }

  fillAuthForm(): void {
    this.pageDispatcher.showPage(AuthFormComponent);
  }
}
