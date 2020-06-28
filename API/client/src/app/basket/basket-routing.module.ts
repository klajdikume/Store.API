import { NgModule } from '@angular/core';
import { BasketComponent } from './basket.component';
import { RouterModule, Routes, Router } from '@angular/router';

const routes: Routes = [
  { path: '', component: BasketComponent }
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class BasketRoutingModule { }
