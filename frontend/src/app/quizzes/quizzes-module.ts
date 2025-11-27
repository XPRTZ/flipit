import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateQuiz } from './create-quiz/create-quiz';
import { SharedModule } from '@shared/shared-module';
import { TakeQuiz } from './take-quiz/take-quiz';
import { Question } from './question/question';
import { CardSidePipe } from './pipes/card-side-pipe';



@NgModule({
  declarations: [
    CreateQuiz,
    TakeQuiz,
    Question,
    CardSidePipe
  ],
  imports: [
    CommonModule,
    SharedModule
  ]
})
export class QuizzesModule { }
