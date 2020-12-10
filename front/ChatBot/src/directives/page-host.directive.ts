import {Directive, ViewContainerRef} from '@angular/core';

@Directive({
  selector: '[pageHost]'
})
export class PageHostDirective {

  constructor(public viewContainerRef: ViewContainerRef) { }

}
