import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import Question from '../../../abstracts/Question';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'first-questions',
  templateUrl: './first-questions.component.html',
  styleUrls: ['./first-questions.component.sass']
})
export class FirstQuestionsComponent implements OnInit {
  @Input() question: Question;
  @Output() select: EventEmitter<Question> = new EventEmitter<Question>();

  constructor() { }

  ngOnInit(): void {
  }

  selectQuestion($event: MouseEvent): void {
    this.select.emit(this.question);
  }
}
