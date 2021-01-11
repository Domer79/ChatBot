import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import User from "../contracts/user";

@Injectable({
  providedIn: 'root'
})
export class OperatorService {

  constructor(
      private httpClient: HttpClient
  ) { }

  getAll(): Observable<User[]>{
    return this.httpClient.get<User[]>('getAllOperators');
  }
}
