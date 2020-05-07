import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PageNotFoundComponent } from './core/page-not-found/page-not-found.component';
import { TillManagerComponent } from './till-manager/till-manager.component';
import { ChangeMakerComponent } from './change-maker/change-maker.component';


const routes: Routes = [
  { path: '', component: TillManagerComponent },
  { path: 'cm', component: ChangeMakerComponent },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
