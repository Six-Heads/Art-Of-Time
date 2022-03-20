import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HorizontalTimelinePageComponent } from './horizontal-timeline-page/horizontal-timeline-page.component';
import {
  HorizontalTimelineComponent
} from "./horizontal-timeline-page/horizontal-timeline/horizontal-timeline.component";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import { HtPageComponent } from './horizontal-timeline-page/horizontal-timeline/ht-page/ht-page.component';

@NgModule({
  declarations: [
    AppComponent,
    HorizontalTimelinePageComponent,
    HorizontalTimelineComponent,
    HtPageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
