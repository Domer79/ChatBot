import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import Question from "../../../abstracts/Question";

@Component({
  selector: 'question-element',
  templateUrl: './question-element.component.html',
  styleUrls: ['./question-element.component.sass']
})
export class QuestionElementComponent implements OnInit {
  @Input() question: Question;
  @Output() responseEditEvent: EventEmitter<Question> = new EventEmitter<Question>();
  @Output() questionDeleteEvent: EventEmitter<Question> = new EventEmitter<Question>();
  @Output() goToChildEvent: EventEmitter<Question> = new EventEmitter<Question>();
  @Output() saveQuestionEvent: EventEmitter<Question> = new EventEmitter<Question>();

  constructor() { }

  ngOnInit(): void {
  }

  edit() {
    this.responseEditEvent.emit(this.question);
  }

  delete() {
    this.questionDeleteEvent.emit(this.question);
  }

  goToChild() {
    this.goToChildEvent.emit(this.question);
  }

  saveQuestion() {
    this.saveQuestionEvent.emit(this.question);
  }
}
