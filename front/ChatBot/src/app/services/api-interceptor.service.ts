import {Injectable, OnDestroy} from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor, HttpHeaders, HttpEventType
} from '@angular/common/http';
import {Observable, Subject} from 'rxjs';
import {environment} from '../../environments/environment';
import {TokenService} from './token.service';
import set = Reflect.set;


@Injectable()
export class ApiInterceptor implements HttpInterceptor, OnDestroy {

  constructor(
      private tokenService: TokenService
  ) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const subject = new Subject<HttpEvent<unknown>>();
    const event = subject.asObservable();
    const regex = /^api\//i;
    let tokenId = '';

    this.tokenService.getToken().then(id => {
      tokenId = id;

      if (request.url.match(regex)){
        const cloneRequest = request.clone({
          url: request.url.replace(request.url, `${environment.apiUrl}/${request.url}`),
          setHeaders: { token: tokenId ?? '' },
        });

        next.handle(cloneRequest).subscribe(_ => subject.next(_));
        return;
      }

      next.handle(request).subscribe(_ => subject.next(_));
    });

    return event;
  }

  ngOnDestroy(): void {
  }
}
