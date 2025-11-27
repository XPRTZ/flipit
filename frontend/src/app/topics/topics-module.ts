import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManageTopics } from './manage-topics/manage-topics';
import { SharedModule } from '@shared/shared-module';
import { ManageCards } from './manage-cards/manage-cards';

@NgModule({
  declarations: [
    ManageTopics,
    ManageCards
  ],
  imports: [
    CommonModule,
    SharedModule
  ]
})
export class TopicsModule { }
