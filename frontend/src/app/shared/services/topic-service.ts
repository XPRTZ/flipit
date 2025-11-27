import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Topic } from '@shared/models/topic';
import { Observable, first, catchError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TopicService {
  private http = inject(HttpClient);
  private snackBar = inject(MatSnackBar);
  
  getTopic(topicId: string) : Observable<Topic | null> {
    return this.http
      .get<Topic | null>(`api/topic/${topicId}`)
      .pipe(
        first(),
        catchError(err => {
          this.snackBar.open('Error loading topics', 'Dismiss', { 
            panelClass: ['error-snackbar']
          });
          throw err;
        })
      );
  }
  
  getTopics() : Observable<Topic[]> {
    return this.http
      .get<Topic[]>('api/topic')
      .pipe(
        first(),
        catchError(err => {
          this.snackBar.open('Error loading topics', 'Dismiss', { 
            panelClass: ['error-snackbar']
          });
          throw err;
        })
      );
  }

  addTopic(topic: Topic) : Observable<Topic> {
    return this.http
      .post<Topic>('api/topic', topic)
      .pipe(
        first(),
        catchError(err => {
          this.snackBar.open('Error adding topic', 'Dismiss', { 
            panelClass: ['error-snackbar']
          });
          throw err;
        })
      );
  }

  editTopic(topic: Topic) : Observable<void> {
    return this.http
      .put<void>(`api/topic/${topic.id}`, topic)
      .pipe(
        first(),
        catchError(err => {
          this.snackBar.open('Error editing topic', 'Dismiss', { 
            panelClass: ['error-snackbar']
          });
          throw err;
        })
      ); 
  }

  deleteTopic(topic: Topic) : Observable<void> {
    return this.http
      .delete<void>(`api/topic/${topic.id}`)
      .pipe(
        first(),
        catchError(err => {
          this.snackBar.open('Error deleting topic', 'Dismiss', { 
            panelClass: ['error-snackbar']
          });
          throw err;
        })
      );
  }  
}
