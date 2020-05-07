import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AddApplicationIdInterceptor implements HttpInterceptor {

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    const jsonReq: HttpRequest<any> = request.clone({
      setHeaders: { ApplicationId: '999', 'Content-Type': 'application/json'}
    });

    return next.handle(jsonReq);
  }
}
