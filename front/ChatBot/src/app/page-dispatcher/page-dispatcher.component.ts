import {Component, ComponentFactoryResolver, EventEmitter, OnDestroy, OnInit, Output, ViewChild} from '@angular/core';
import Page from '../../abstracts/Page';
import {PageDispatcherService} from '../services/page-dispatcher.service';
import {PageHostDirective} from '../../directives/page-host.directive';
import {Subscription} from 'rxjs';

@Component({
  selector: 'page-dispatcher',
  templateUrl: './page-dispatcher.component.html',
  styleUrls: ['./page-dispatcher.component.sass']
})
export class PageDispatcherComponent implements OnInit, OnDestroy {
  private pageSubscription: Subscription;

  @ViewChild(PageHostDirective, {static: true}) pageHost: PageHostDirective;

  constructor(private pageDispatcher: PageDispatcherService,
              private componentFactoryResolver: ComponentFactoryResolver) { }

  ngOnInit(): void {
    this.pageSubscription = this.pageDispatcher.getPage().subscribe(page => this.loadComponent(page));
  }

  private loadComponent(page: Page): void{
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(page.component);
    const viewContainerRef = this.pageHost.viewContainerRef;
    viewContainerRef.clear();
    const componentRef = viewContainerRef.createComponent(componentFactory);
    this.pageDispatcher.setCurPageInstance(componentRef.instance);
    componentRef.instance.data = page.data;
  }

  ngOnDestroy(): void {
    this.pageSubscription.unsubscribe();
  }
}
