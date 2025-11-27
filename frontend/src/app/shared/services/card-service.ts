import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Card } from '@shared/models/card';
import { Observable, first, catchError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CardService {
  private http = inject(HttpClient);
  private snackBar = inject(MatSnackBar);  
    
  getCards(topicId : string) : Observable<Card[]> {
    return this.http
      .get<Card[]>(`api/topic/${topicId}/card`)
      .pipe(
        first(),
        catchError(err => {
          this.snackBar.open('Error loading cards', 'Dismiss', { 
            panelClass: ['error-snackbar']
          });
          throw err;
        })
      );
  }

  addCard(topicId: string, card: Card) : Observable<Card> {
    return this.http
      .post<Card>(`api/topic/${topicId}/card`, card)
      .pipe(
        first(),
        catchError(err => {
          this.snackBar.open('Error adding card', 'Dismiss', { 
            panelClass: ['error-snackbar']
          });
          throw err;
        })
      );
  }

  editCard(topicId: string, card: Card) : Observable<void> {
    return this.http
      .put<void>(`api/topic/${topicId}/card/${card.id}`, card)
      .pipe(
        first(),
        catchError(err => {
          this.snackBar.open('Error editing card', 'Dismiss', { 
            panelClass: ['error-snackbar']
          });
          throw err;
        })
      );
  }

  deleteCard(topicId: string, card: Card) : Observable<void> {
    return this.http
      .delete<void>(`api/topic/${topicId}/card/${card.id}`)
      .pipe(
        first(),
        catchError(err => {
          this.snackBar.open('Error deleting card', 'Dismiss', { 
            panelClass: ['error-snackbar']
          });
          throw err;
        })
      );
  }  
}
