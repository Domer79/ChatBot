import {Component, OnInit, Output, EventEmitter} from '@angular/core';
import {PageDispatcherService} from '../services/page-dispatcher.service';

@Component({
  selector: 'chat-header',
  templateUrl: './chat-header.component.html',
  styleUrls: ['./chat-header.component.sass']
})
export class ChatHeaderComponent implements OnInit {
  @Output() closed = new EventEmitter<void>();
  @Output() openQuestions = new EventEmitter<void>();

  constructor() { }

  ngOnInit(): void {
  }

  onOpenQuestions($event: MouseEvent): void {
    this.openQuestions.emit();
  }

  onClosed($event: MouseEvent): void {
    this.closed.emit();
  }
}
