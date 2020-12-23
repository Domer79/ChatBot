import {Component, ComponentFactoryResolver, OnDestroy, OnInit, ViewChild} from '@angular/core';
import Page from '../../abstracts/Page';
import {ChatDialogComponent} from '../dialog/chat-dialog.component';
import {PageDispatcherService} from '../services/page-dispatcher.service';
import {PageHostDirective} from '../../directives/page-host.directive';
import {Observable, Subscription} from 'rxjs';

@Component({
  selector: 'page-dispatcher',
  templateUrl: './page-dispatcher.component.html',
  styleUrls: ['./page-dispatcher.component.sass']
})
export class PageDispatcherComponent implements OnInit, OnDestroy {
  @ViewChild(PageHostDirective, {static: true}) pageHost: PageHostDirective;
  private pageSubscription: Subscription;
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
  }

  ngOnDestroy(): void {
    this.pageSubscription.unsubscribe();
  }
}
