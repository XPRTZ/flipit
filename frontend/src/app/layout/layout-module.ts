import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';

import { Layout } from './layout/layout';
import { Navbar } from './navbar/navbar';
import { SharedModule } from '@shared/shared-module';
import { TopicsModule } from '@topics/topics-module';
import { QuizzesModule } from '@quizzes/quizzes-module';
import { HomeModule } from '@home/home-module';

@NgModule({
  declarations: [
    Layout,
    Navbar
  ],
  imports: [
    CommonModule, 
    RouterModule,
    MatToolbarModule,
    SharedModule,
    HomeModule,
    TopicsModule,
    QuizzesModule
  ]
})
export class LayoutModule { }
