import {Type} from '@angular/core';

export interface BackService{
  isShowBack(): boolean;
  isBackWithClose(): boolean;
  goBack(): void;
}

export interface HasBackService{
  getBackService(): BackService;
}
