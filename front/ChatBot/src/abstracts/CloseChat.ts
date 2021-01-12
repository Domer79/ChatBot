import {EventEmitter} from '@angular/core';

export default interface CloseChat {
  passClosedEmitter(closed: EventEmitter<void>): void;
}
