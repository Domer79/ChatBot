import { Injectable } from '@angular/core';

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

  clear(): void{
    sessionStorage.clear();
  }
}

class CacheValue<T>{
  constructor(value: T){
    this.value = value;
  }
  value: T;
}
