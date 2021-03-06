import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {BehaviorSubject, Observable, of} from "rxjs";
import {CacheService} from "./cache.service";
import {delay, exhaustMap, map, publishReplay, refCount, switchMap, tap} from "rxjs/operators";
import Token from "../contracts/token";
import {TokenService} from "./token.service";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private userPolicies: Observable<string[]>;
  redirectUrl: string = '';

  constructor(
      private httpClient: HttpClient,
      private cache: CacheService,
      private tokenService: TokenService
  ) { }

  get isLoggedIn(): boolean{
      return this.tokenService.exist();
  }

  checkAccess(policy: string): Observable<boolean>{
    if (this.cache.contains(policy))
      return of(this.cache.get<boolean>(policy));

    return this.httpClient.get<boolean>("api/Auth/CheckAccess", { params: { policy } })
        .pipe(tap(_ => this.cache.set(policy, _)));
  }

  logIn(login: string, password: string): Observable<boolean> {
    return this.httpClient.post<any>("api/Auth/Login", {
      login: login,
      password
    }).pipe(
        map(_ => {
          if (_ == null)
            return false;

          this.tokenService.saveToken(new Token(_));
          this.userPolicies = null;
          return true;
        })
    );
  }

  checkAccessPolicy(policy: string): Observable<boolean> {
      if (!this.userPolicies){
          this.userPolicies = this.httpClient.get<string[]>('api/Auth/GetAllUserPolicies').pipe(
              publishReplay(1),
              refCount()
          );
      }

      return this.userPolicies.pipe(
          map(policies => {
              return policies.some(_ => _ === policy);
          }),
      );
  }
}
