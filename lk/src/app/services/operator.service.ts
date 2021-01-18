import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import User from "../contracts/user";
import Page from "../contracts/Page";
import {Role} from "../contracts/role";

@Injectable({
  providedIn: 'root'
})
export class OperatorService {

  constructor(
      private httpClient: HttpClient
  ) { }

  getPage(number: number, size: number, isActive?: boolean): Observable<Page<User>>{
    const options = { params: new HttpParams().set('number', ''+number).set('size', ''+size) };
    if (isActive){
      options.params.set('isActive', isActive.toString());
    }
    return this.httpClient.get<Page<User>>('api/User/GetPage', options);
  }

  upsert(user: User): Promise<User>{
    return this.httpClient.post<User>('api/Operator/Upsert', user).toPromise();
  }

  setPassword(userId: string, password: string): Promise<boolean>{
    return this.httpClient.post<boolean>('api/Auth/SetPassword', {userId, password}).toPromise();
  }

  getRoles(userId: string): Observable<Role[]>{
    return this.httpClient.get<Role[]>('api/Role/GetRoles', {params: {userId}});
  }

  async setRole(roleId: string, userId: string, checked: boolean): Promise<boolean> {
    return this.httpClient.put<boolean>('api/Role/SetRole', {roleId, userId, set: checked}).toPromise();
  }
}
