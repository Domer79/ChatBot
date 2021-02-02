import {Type} from '@angular/core';

export interface BackService{
  isShowBack(): boolean;
  goBack(): void;
  getComponent(): Type<any>;
}
