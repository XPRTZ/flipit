import { NgModule, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core';
import { BrowserModule, provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { provideHttpClient, withFetch } from '@angular/common/http';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { LayoutModule } from '@layout/layout-module';
import { SharedModule } from '@shared/shared-module';
import { TopicsModule } from '@topics/topics-module';
import { QuizzesModule } from '@quizzes/quizzes-module';
import { HomeModule } from '@home/home-module';
import { MAT_SNACK_BAR_DEFAULT_OPTIONS } from '@angular/material/snack-bar';

@NgModule({
  declarations: [
    App
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    LayoutModule,
    SharedModule,
    TopicsModule,
    QuizzesModule,
    HomeModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideClientHydration(withEventReplay()),
    provideHttpClient(withFetch()),
    {provide: MAT_SNACK_BAR_DEFAULT_OPTIONS, useValue: {duration: 2000}}
  ],
  bootstrap: [App]
})
export class AppModule { }
