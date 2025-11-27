import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ItemColumn } from '@shared/models/item-column';
import { Topic } from '@shared/models/topic';
import { TopicService } from '@shared/services/topic-service';
import { Card } from '@shared/models/card';
import { catchError, first } from 'rxjs';
import { CardService } from '@shared/services/card-service';

@Component({
  selector: 'app-manage-cards',
  standalone: false,
  templateUrl: './manage-cards.html',
  styleUrl: './manage-cards.sass',
})
export class ManageCards {
  private route = inject(ActivatedRoute);
  private topicService = inject(TopicService);
  private cardService = inject(CardService);

  topicId = this.route.snapshot.paramMap.get('topicId')!;
  topic = signal<Topic | null>(null);
  cards = signal<Card[]>([]);

  ngOnInit() {
    this.loadTopic();
    this.loadCards();
  }

  loadTopic() {
    this.topicService
      .getTopic(this.topicId)
      .pipe(
        catchError(err => {
          this.topic.set(null);
          throw err;
        })
      )
      .subscribe(topic => {
        this.topic.set(topic);
      });
  }

  loadCards() {
    this.cardService
      .getCards(this.topicId)
      .pipe(
        first()
      )
      .subscribe(cards => {
        this.cards.set(cards);
      });
  }

  getColumns() : ItemColumn[] {
    return [
      { name: 'front', label: 'Front Text', editable: true },
      { name: 'back', label: 'Back Text', editable: true },
      { name: 'correctnessRatio', label: 'Answered Correctly', editable: false },
      { name: 'lastViewedAt', label: 'Last Viewed', editable: false }
    ];
  }

  addCard(item: object) {
    const card = item as Card;
    this.cardService
      .addCard(this.topicId, card)
      .pipe(
        catchError(err => {
          this.loadCards();
          throw err;
        })
      )
      .subscribe(_ => this.loadCards());
  }

  editCard(item: object) {
    const card = item as Card;
    this.cardService
      .editCard(this.topicId, card)
      .pipe(
        catchError(err => {
          this.loadCards();
          throw err;
        })
      )
      .subscribe(_ => this.loadCards());
  }

  deleteCard(item: object) {
    const card = item as Card;
    this.cardService
      .deleteCard(this.topicId, card)
      .pipe(
        catchError(err => {
          this.loadCards();
          throw err;
        })
      )
      .subscribe(_ => this.loadCards());
  }
}
