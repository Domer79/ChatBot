import {Component, OnInit, Output, EventEmitter, Input} from '@angular/core';

@Component({
  selector: 'chat-bot-button',
  templateUrl: './chat-bot-button.component.html',
  styleUrls: ['./chat-bot-button.component.sass']
})
export class ChatBotButtonComponent implements OnInit {
  @Output() toggle = new EventEmitter<boolean>();

  constructor() { }

  ngOnInit(): void {
  }

  clickBtn(){
    this.toggle.emit();
  }
}
