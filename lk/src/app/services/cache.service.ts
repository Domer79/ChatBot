import { Injectable } from '@angular/core';
import {stringify} from "querystring";
import {Observable} from "rxjs";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class CacheService {

  constructor() {

  }

  set(key: string, value: any): void{
    const result = new CacheValue(value);
    const s = JSON.stringify(result);
    sessionStorage.setItem(key, s);
  }

  get<T>(key: string): T {
    const s = sessionStorage.getItem(key);
    if (s == undefined)
      throw new Error(`Key '${key}' in cache not found`);

    const cacheValue = JSON.parse(s) as CacheValue<T>;
    return cacheValue.value;
  }

  contains(key: string): boolean {
    const result = sessionStorage.getItem(key) != null;
    return result;
  }
}

class CacheValue<T>{
  constructor(value: T){
    this.value = value;
  }
  value: T;
}
