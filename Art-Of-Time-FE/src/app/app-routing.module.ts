import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import {HtPageComponent} from "./horizontal-timeline-page/horizontal-timeline/ht-page/ht-page.component";

const routes: Routes = [
  {
    path: '',
    component: AppComponent,
  },
  {
    path: ':id',
    component: HtPageComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
