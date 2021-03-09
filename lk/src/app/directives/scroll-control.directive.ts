import {Directive, ElementRef, HostListener, OnDestroy} from '@angular/core';
import {ViewportScroller} from '@angular/common';

@Directive({
  selector: '[scrollControl]'
})
export class ScrollControlDirective implements OnDestroy{
  private changes: MutationObserver;

  constructor(
    private el: ElementRef,
    private viewPortScroller: ViewportScroller) {
    const element = this.el.nativeElement;

    this.changes = new MutationObserver((mutations: MutationRecord[]) => {
        console.log(element);
        if (element.scrollHeight > 400) {
          const p: any = mutations[0].addedNodes[0];
          setTimeout(() => {
            viewPortScroller.scrollToAnchor(p.id);
            element.scrollTo(0, 10000);
          }, 50);
        }
      }
    );

    this.changes.observe(element, {
      childList: true,
    });
  }

  @HostListener('scroll') onScroll(): void{
    const scroll = this.el.nativeElement.scroll;
  }

  ngOnDestroy(): void {
    this.changes.disconnect();
  }

}
