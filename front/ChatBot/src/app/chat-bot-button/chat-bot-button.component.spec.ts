import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChatBotButtonComponent } from './chat-bot-button.component';

describe('ChatBotButtonComponent', () => {
  let component: ChatBotButtonComponent;
  let fixture: ComponentFixture<ChatBotButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChatBotButtonComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChatBotButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
