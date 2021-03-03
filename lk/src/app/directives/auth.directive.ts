import {ChangeDetectorRef, Directive, Input, TemplateRef, ViewContainerRef} from '@angular/core';
import {AsyncSubject, Observable, of, Subject} from "rxjs";
import {concatMapTo, switchMap, takeUntil} from "rxjs/operators";
import {AuthService} from "../services/auth.service";

@Directive({
  selector: '[auth]'
})
export class AuthDirective {
  private errorRef: TemplateRef<any>;
  private unsubscribe = new Subject<boolean>();
  private init = new AsyncSubject<void>();

  constructor(
      private nextRef: TemplateRef<any>,
      private view: ViewContainerRef,
      private changes: ChangeDetectorRef,
      private authService: AuthService,
  ) { }

  @Input()
  set auth(policy: string) {
    if (!policy) {
      return
    }
    this.showBefore()
    this.unsubscribe.next(true);
    this.init.pipe(
        switchMap(() => {
          let booleanObservable = this.authService.checkAccessPolicy(policy);
          return booleanObservable;
        }),
        takeUntil(this.unsubscribe)
    ).subscribe(access => {
      this.view.clear()
      if (access){
        this.view.createEmbeddedView(this.nextRef, {$implicit: access, observe: access})
        this.changes.markForCheck()
      }
    }, error => {
      if (this.errorRef) {
        this.view.clear()
        this.view.createEmbeddedView(this.errorRef, {$implicit: error})
        this.changes.markForCheck()
      }
    })
  }

  @Input()
  set authError(ref: TemplateRef<any>) {
    this.errorRef = ref;
  }

  ngOnDestroy() {
    this.unsubscribe.next(true)
  }

  ngOnInit() {
    this.showBefore()
    this.init.next()
    this.init.complete()
  }

  private showBefore(): void {
    this.view.clear()
  }
}
