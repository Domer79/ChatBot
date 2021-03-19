import { Component, OnInit } from '@angular/core';
import {ChatManagerService} from '../services/chat-manager.service';

@Component({
  selector: 'chat-manager',
  templateUrl: './chat-manager.component.html',
  styleUrls: ['./chat-manager.component.sass'],
})
export class ChatManagerComponent implements OnInit {
  opened = false;

  constructor(
    private chatManagerService: ChatManagerService
  ) { }

  ngOnInit(): void {
  }

  onToggle(): void {
    this.opened = !this.opened;
    this.chatManagerService.setOpenChat(this.opened);
    console.log(`Chat opened: ${this.opened}`);
  }

  onClosed(): void {
    this.opened = false;
    this.chatManagerService.setOpenChat(this.opened);
  }
}
