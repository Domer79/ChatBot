import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor, HttpHeaders
} from '@angular/common/http';
import { Observable } from 'rxjs';
import {environment} from "../../environments/environment";
import {TokenService} from "./token.service";


@Injectable()
export class ApiInterceptor implements HttpInterceptor {

  constructor(
      private tokenService: TokenService
  ) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    var regex = /^api\//i;
    if (request.url.match(regex)){
      const cloneRequest = request.clone({
        withCredentials: true,
        url: request.url.replace(request.url, `${environment.apiUrl}/${request.url}`),
        setHeaders: { token: this.tokenService.tokenId ?? "" },
      });

      return next.handle(cloneRequest);
    }

    return next.handle(request);
  }
}
