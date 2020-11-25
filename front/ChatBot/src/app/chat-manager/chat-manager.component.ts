import { Component, OnInit } from '@angular/core';
import {animate, state, style, transition, trigger} from "@angular/animations";

@Component({
  selector: 'chat-manager',
  templateUrl: './chat-manager.component.html',
  styleUrls: ['./chat-manager.component.sass'],
})
export class ChatManagerComponent implements OnInit {
  opened: boolean = false;

  constructor() { }

  ngOnInit(): void {
  }

  onToggle() {
    this.opened = !this.opened;
  }

  onClosed() {
    this.opened = false;
  }
}
