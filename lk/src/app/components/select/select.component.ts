import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'rao-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.sass']
})
export class SelectComponent implements OnInit {
  items: string[] = ['One', 'Two', 'Three', 'Four'];
  selectedItem: string = undefined;
  isOpen = false;

  constructor() { }

  ngOnInit(): void {
  }

  selectItem(item: string) {
    this.selectedItem = item;
    this.isOpen = false;
  }
}
