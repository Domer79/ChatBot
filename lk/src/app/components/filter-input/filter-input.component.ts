import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'rao-filter-input',
  templateUrl: './filter-input.component.html',
  styleUrls: ['./filter-input.component.sass', '../filter-elements.sass']
})
export class FilterInputComponent implements OnInit {
  filter: string = undefined;

  constructor() { }

  ngOnInit(): void {
  }

}
