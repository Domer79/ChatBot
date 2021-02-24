import {
  AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  ViewChild
} from '@angular/core';

@Component({
  selector: 'rao-filter-input',
  templateUrl: './filter-input.component.html',
  styleUrls: ['./filter-input.component.sass', '../filter-elements.sass']
})
export class FilterInputComponent implements OnInit, OnDestroy, AfterViewInit {
  private isViewInit = false;

  @Input() placeholder: string = null;
  @Input() filter: any = '';
  @Output() filterChange: EventEmitter<any> = new EventEmitter<any>();
  @ViewChild('editor') editor: ElementRef;

  constructor() { }

  ngOnInit(): void {
  }

  setFilter($event: FocusEvent) {
    // this.filter = '';
    this.filterChange.emit(this.filter);
  }

  get isPlaceholder(): boolean{
    return this.isViewInit && (this.filter === undefined || this.filter === '');
  }

  ngAfterViewInit(): void {
    setTimeout(() => this.isViewInit = true, 1000);
  }

  ngOnDestroy(): void {
  }
}
