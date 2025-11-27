import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Layout } from '@layout/layout/layout';
import { Home } from '@home/home/home';
import { ManageTopics } from '@topics/manage-topics/manage-topics';
import { CreateQuiz } from '@quizzes/create-quiz/create-quiz';
import { ManageCards } from '@topics/manage-cards/manage-cards';
import { TakeQuiz } from '@quizzes/take-quiz/take-quiz';

const routes: Routes = [
  {
    path: '',
    component: Layout,
    children: [
      {
        path: '',
        component: Home
      },
      {
        path: 'topics',
        component: ManageTopics
      },
      {
        path: 'topics/:topicId/cards',
        component: ManageCards
      },
      {
        path: 'quizzes',
        component: CreateQuiz
      },
      {
        path: 'quizzes/:quizId',
        component: TakeQuiz
      }
    ],
  },];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
