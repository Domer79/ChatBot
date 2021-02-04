import {ComponentRef, Type} from '@angular/core';

export default class Page{
  constructor(public component: Type<any>){

  }

  public data: any = null;
  public instance: ComponentRef<any> = null;

  get componentName(): string{
    return this.component.name;
  }
}
