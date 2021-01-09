import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor, HttpHeaders
} from '@angular/common/http';
import { Observable } from 'rxjs';
import {environment} from '../../environments/environment';
import {TokenService} from './token.service';
import set = Reflect.set;


@Injectable()
export class ApiInterceptor implements HttpInterceptor {

  constructor(
      private tokenService: TokenService
  ) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const regex = /^api\//i;
    let tokenId = '';
    this.tokenService.getToken().then(id => {
      tokenId = id;
    });

    if (request.url.match(regex)){
      const cloneRequest = request.clone({
        url: request.url.replace(request.url, `${environment.apiUrl}/${request.url}`),
        setHeaders: { token: tokenId ?? '' },
      });

      return next.handle(cloneRequest);
    }

    return next.handle(request);
  }
}
