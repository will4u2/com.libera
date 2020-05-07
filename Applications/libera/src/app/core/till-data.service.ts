import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { IStandardError } from '../_models/i-standard-error';
import { Observable, throwError } from 'rxjs';
import { ICoin } from '../_models/i-coin';
import { environment } from 'src/environments/environment';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TillDataService {

  constructor(private http: HttpClient) { }

  getTill(): Observable<ICoin[] | IStandardError> {
    return this.http.get<ICoin[]>(environment.apiBaseUrl + '/till')
      .pipe(
        catchError(err => this.handleHttpError(err))
      );
  }

  getChange(amount: number): Observable<ICoin[] | IStandardError> {
    return this.http.get<ICoin[]>(environment.apiBaseUrl + '/till/change/' + amount)
      .pipe(
        catchError(err => this.handleHttpError(err))
      );
  }

  patchTill(coins: ICoin[]) {
    return this.http.patch(environment.apiBaseUrl + '/till', coins);
  }

  private handleHttpError(error: HttpErrorResponse): Observable<IStandardError> {
    const dataError = {
      title: 'Till Dataservice Error',
      displayMessage: 'Was not able to retrieve till.',
      errorMessage: error.statusText
    } as IStandardError;
    return throwError(dataError);
  }
}
