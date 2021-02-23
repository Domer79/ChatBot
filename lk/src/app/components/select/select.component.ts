import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

@Component({
  selector: 'rao-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.sass', '../filter-elements.sass']
})
export class SelectComponent implements OnInit {
  @Input() selectPattern: { 'key': string, 'name': string }
  @Input() items: any[];
  @Input() selectedItem: any;
  @Output()selectedItemChange: EventEmitter<any> = new EventEmitter<any>();

  isOpen = false;

  constructor() { }

  ngOnInit(): void {
  }

  selectItem(item: any) {
    this.selectedItem = item;
    this.isOpen = false;
  }
}
