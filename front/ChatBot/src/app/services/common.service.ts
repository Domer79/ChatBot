import { Injectable } from '@angular/core';
import {Time} from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class CommonService {
  closingShift: Time = { hours: 18, minutes: 0 };
  beginShift: Time = { hours: 9, minutes: 0 };

  constructor() { }
}
