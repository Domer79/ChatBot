import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable, of} from "rxjs";
import {CacheService} from "./cache.service";
import {map, tap} from "rxjs/operators";
import Token from "../contracts/token";
import {TokenService} from "./token.service";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  redirectUrl: string = '';

  constructor(
      private httpClient: HttpClient,
      private cacheService: CacheService,
      private tokenService: TokenService
  ) { }

  get isLoggedIn(): boolean{
      return this.tokenService.exist;
  }

  checkAccess(policy: string): Observable<boolean>{
    if (this.cacheService.contains(policy))
      return of(this.cacheService.get<boolean>(policy));

    debugger;
    return this.httpClient.get<boolean>("api/Auth/CheckAccess", { params: { policy } })
        .pipe(tap(_ => this.cacheService.set(policy, _)));
  }

  logIn(login: string, password: string): Observable<boolean> {
    debugger;
    return this.httpClient.post<any>("api/Auth/Login", {
      loginOrEmail: login,
      password
    }).pipe(
        map(_ => {
          if (_ == null)
            return false;

          this.tokenService.saveToken(new Token(_));
          return true;
        })
    );
  }
}
