import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { QuizQuestion } from '@quizzes/models/quiz-question';
import { Quiz } from '@quizzes/models/quiz';
import { Observable, first, catchError, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class QuizService {
  private http = inject(HttpClient);
  private snackBar = inject(MatSnackBar);

  createQuiz(quiz: Quiz): Observable<Quiz> {
    return this.http
      .post<Quiz>(`api/quiz`, quiz)
      .pipe(
        first(),
        catchError(err => {
          this.snackBar.open('Error creating quiz', 'Dismiss', {
            panelClass: ['error-snackbar']
          });
          throw err;
        })
      );
  }

  getQuiz(quizId: string): Observable<Quiz | null> {
    return this.http
      .get<Quiz | null>(`api/quiz/${quizId}`)
      .pipe(
        first(),
        catchError(err => {
          this.snackBar.open('Error loading quiz', 'Dismiss', {
            panelClass: ['error-snackbar']
          });
          throw err;
        })
      );
  }

  getQuizzes(): Observable<Quiz[]> {
    return this.http
      .get<Quiz[]>(`api/quiz`)
      .pipe(
        first(),
        catchError(err => {
          this.snackBar.open('Error loading quizzes', 'Dismiss', {
            panelClass: ['error-snackbar']
          });
          throw err;
        })
      );
  }

  getNextQuestion(quizId: string): Observable<QuizQuestion | null> {
    return this.http
      .get<QuizQuestion | null>(`api/quiz/${quizId}/next-question`)
      .pipe(
        first(),
        catchError(err => {
          if (err.status === 404) {
            return of(null);
          }
          this.snackBar.open('Error loading next question', 'Dismiss', {
            panelClass: ['error-snackbar']
          });
          throw err;
        })
      );
  }

  answerQuestion(quizId: string, questionId: string, isCorrect: boolean): Observable<void> {
    return this.http
      .post<void>(`api/quiz/${quizId}/answer-question`, { questionId, isCorrect })
      .pipe(
        first(),
        catchError(err => {          
          this.snackBar.open('Error submitting answer', 'Dismiss', {
            panelClass: ['error-snackbar']
          });
          throw err;
        })
      );
  }
}
