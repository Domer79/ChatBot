import { Injectable } from '@angular/core';
import {Time} from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class CommonService {
  beginShift: Time = { hours: 0, minutes: 0 };
  closingShift: Time = { hours: 24, minutes: 0 };

  constructor() { }
}
