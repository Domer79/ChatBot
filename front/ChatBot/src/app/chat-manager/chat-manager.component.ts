import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'chat-manager',
  templateUrl: './chat-manager.component.html',
  styleUrls: ['./chat-manager.component.sass'],
})
export class ChatManagerComponent implements OnInit {
  opened = false;

  constructor() { }

  ngOnInit(): void {
  }

  onToggle(): void {
    this.opened = !this.opened;
    console.log(`Chat opened: ${this.opened}`);
  }

  onClosed(): void {
    this.opened = false;
  }
}
