import {AfterViewInit, Component, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild} from '@angular/core';
import {MatDatepickerInputEvent} from "@angular/material/datepicker";

@Component({
  selector: 'rao-datepicker',
  templateUrl: './date-picker.component.html',
  styleUrls: ['./date-picker.component.sass', '../filter-elements.sass']
})
export class DatePickerComponent implements OnInit, OnDestroy, AfterViewInit {
  @Input() placeholder: string = null;
  @Input() date: Date = null;
  @Output() dateChange: EventEmitter<Date> = new EventEmitter<Date>();

  constructor() {
  }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
  }

  ngOnDestroy(): void {
  }

  setDate($event: MatDatepickerInputEvent<Date, Date | null>) {
    this.date = $event.value;
    this.dateChange.emit($event.value);
  }
}
