import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {QuestionsProviderService} from '../../services/questions-provider.service';

@Component({
  selector: 'questions-header',
  templateUrl: './questions-header.component.html',
  styleUrls: ['./questions-header.component.sass']
})
export class QuestionsHeaderComponent implements OnInit {
  @Input() responsePreset: boolean;
  @Output() close: EventEmitter<any> = new EventEmitter<any>();

  constructor(private questionsProvider: QuestionsProviderService) { }

  ngOnInit(): void {
  }

  stopShowResponse(): void {
    this.questionsProvider.stopShowResponse();
  }

  closePage(): void {
    this.close.emit();
  }
}
