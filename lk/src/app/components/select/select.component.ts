import {AfterViewInit, Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';

@Component({
  selector: 'rao-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.sass', '../filter-elements.sass']
})
export class SelectComponent implements OnInit, AfterViewInit {
  private _isOpen = false;
  @Input() placeholder: string = null;
  @Input() selectPattern: { 'key': string, 'name': string } = {key: 'key', name: 'name'};
  @Input() items: any[];
  @Input() selectedItem: any;
  @Output() selectedItemChange: EventEmitter<any> = new EventEmitter<any>();
  @ViewChild('filterValueContainer') filterValueContainer: ElementRef;

  componentTop: string;
  componentLeft: string;
  componentWidth: string;

  constructor() { }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    this.updateItemsContainerSize();
  }

  selectItem(item: any) {
    this.selectedItemChange.emit(item);
    this.isOpen = false;
  }

  get isOpen(){
    return this._isOpen;
  }

  set isOpen(value){
    this._isOpen = value;
    this.updateItemsContainerSize();
  }

  private updateItemsContainerSize(){
    const domRect = this.filterValueContainer.nativeElement.getBoundingClientRect();
    this.componentLeft = `${domRect.left}px`;
    this.componentTop = `${domRect.top + 54}px`;
    this.componentWidth = `${domRect.width}px`;
  }
}
