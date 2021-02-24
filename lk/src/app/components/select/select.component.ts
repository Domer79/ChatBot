import {AfterViewInit, Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';

@Component({
  selector: 'rao-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.sass', '../filter-elements.sass']
})
export class SelectComponent implements OnInit, AfterViewInit {
  @Input() selectPattern: { 'key': string, 'name': string } = {key: 'key', name: 'name'};
  @Input() items: any[];
  @Input() selectedItem: any;
  @Output() selectedItemChange: EventEmitter<any> = new EventEmitter<any>();
  @ViewChild('filterValueContainer') filterValueContainer: ElementRef;

  isOpen = false;
  componentTop: string;
  componentLeft: string;
  componentWidth: string;

  constructor() { }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    const domRect = this.filterValueContainer.nativeElement.getBoundingClientRect();
    this.componentLeft = `${domRect.left}px`;
    this.componentTop = `${domRect.top + 54}px`;
    this.componentWidth = `${domRect.width}px`;
    console.log(domRect);
    console.log(this.componentLeft, this.componentTop);
  }

  selectItem(item: any) {
    this.selectedItem = item;
    this.isOpen = false;
  }
}
