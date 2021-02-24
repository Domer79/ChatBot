import {AfterViewInit, Component, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild} from '@angular/core';
import {MatDatepickerInputEvent} from "@angular/material/datepicker";
import * as _moment from 'moment';
// tslint:disable-next-line:no-duplicate-imports
// @ts-ignore
import {default as _rollupMoment} from 'moment';

const moment = _rollupMoment || _moment;

@Component({
  selector: 'rao-datepicker',
  templateUrl: './date-picker.component.html',
  styleUrls: ['./date-picker.component.sass', '../filter-elements.sass']
})
export class DatePickerComponent implements OnInit, OnDestroy, AfterViewInit {
  @Input() placeholder: string = null;
  @Input() date: string = null;
  @Output() dateChange: EventEmitter<string> = new EventEmitter<string>();

  constructor() {
  }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
  }

  ngOnDestroy(): void {
  }

  setDate($event: MatDatepickerInputEvent<Date, Date | null>) {
    this.date = moment($event.value).utc().format();
    this.dateChange.emit(this.date);
  }
}
