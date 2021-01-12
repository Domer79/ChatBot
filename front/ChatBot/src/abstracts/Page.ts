import {Type} from '@angular/core';

export default class Page{
  constructor(public component: Type<any>){

  }

  get componentName(): string{
    return this.component.name;
  }
}
