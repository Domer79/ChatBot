import {Injectable, OnDestroy, Type} from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanActivateChild,
  Router,
  RouterStateSnapshot,
  UrlTree
} from "@angular/router";
import {Observable, of, Subscription} from "rxjs";
import User from "./contracts/user";
import {AuthService} from "./auth.service";
import {tap} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate, CanActivateChild, OnDestroy {
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot)
      : Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.checkAccess(route, state);
  }

  constructor(
      private router: Router,
      private authService: AuthService
  ) {
  }

  ngOnDestroy(): void {
  }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot)
      : Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.checkAccess(childRoute, state);
  }

  private isAuth(component: any): boolean{
    return !!component['__securityPolicy'];
  }

  private checkAccess(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean>{
    if (!this.authService.isLoggedIn) {
      this.authService.redirectUrl = state.url;
      this.router.navigate(['/login']);
    }

    const component = route.component;
    if (this.isAuth(component)){
      return this.authService.checkAccess(component['__securityPolicy'] as string)
          .pipe(tap(async result => {
            if (!result)
              await this.router.navigate(['/denied']);

            return result;
          }));
    }

    return of(true);
  }
}
