import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PageNotFoundComponent } from './core/page-not-found/page-not-found.component';
import { TillManagerComponent } from './till-manager/till-manager.component';
import { ChangeMakerComponent } from './change-maker/change-maker.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { StandardReplyInterceptor } from './core/standard-reply.interceptor';
import { AddApplicationIdInterceptor } from './core/add-application-id.interceptor';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';

@NgModule({
  declarations: [
    AppComponent,
    PageNotFoundComponent,
    TillManagerComponent,
    ChangeMakerComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatSelectModule,
    MatTableModule,
    MatPaginatorModule
  ],
  exports: [
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: StandardReplyInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AddApplicationIdInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
