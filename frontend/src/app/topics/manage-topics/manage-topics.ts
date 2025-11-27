import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { ItemColumn } from '@shared/models/item-column';
import { Topic } from '@shared/models/topic';
import { TopicService } from '@shared/services/topic-service';
import { Card } from '@shared/models/card';

import { catchError, first, forkJoin, Observable } from 'rxjs';
import { CardService } from '@shared/services/card-service';

type CardMap = {
  [topicId: string]: Observable<Card[]>
};

@Component({
  selector: 'app-manage-topics',
  standalone: false,
  templateUrl: './manage-topics.html',
  styleUrl: './manage-topics.sass',
})
export class ManageTopics {
  private router = inject(Router);
  private topicService = inject(TopicService);
  private cardService = inject(CardService);

  topics = signal<Topic[]>([]);

  ngOnInit() {
    this.loadTopics();
  }

  getColumns() : ItemColumn[] {
    return [
      { name: 'name', label: 'Topic Name', editable: true },
      { name: 'frontLabel', label: 'Front Label', editable: true },
      { name: 'backLabel', label: 'Back Label', editable: true },
      { name: 'numberOfCards', label: 'Number of Cards', editable: false }
    ];
  }

  loadTopics() {
    this.topicService
      .getTopics()
      .pipe(
        catchError(err => {
          this.topics.set([]);
          throw err;
        })
      )
      .subscribe(topics => {
        this.topics.set(topics);
        this.loadCards();
      });
  }

  loadCards() {
    let results: CardMap = {};
    for (let topic of this.topics()) {
      results[topic.id] = this.cardService.getCards(topic.id);
    }

    forkJoin(results)
      .pipe(first())
      .subscribe(cardMap => {
        this.topics.update(topics =>
          topics.map(topic => {
            topic.numberOfCards = cardMap[topic.id].length;
            return topic;
          }));
      });
  }

  addTopic(item: object) {
    const topic = item as Topic;
    this.topics.update(x => [...x, topic]);
    this.topicService
      .addTopic(topic)
      .pipe(
        catchError(err => {
          this.loadTopics();
          throw err;
        })
      )
      .subscribe(_ => this.loadTopics());
  }

  editTopic(item: object) {
    const topic = item as Topic;
    this.topicService
      .editTopic(topic)
      .pipe(
        catchError(err => {
          this.loadTopics();
          throw err;
        })
      )
      .subscribe(_ => this.loadTopics());
  }

  deleteTopic(item: object) {
    const topic = item as Topic;
    this.topics.update(x => x.filter(t => t.id !== topic.id));
    this.topicService
      .deleteTopic(topic)
      .pipe(
        catchError(err => {
          this.loadTopics();
          throw err;
        })
      )
      .subscribe(_ => this.loadTopics());
  }

  goToCards(item: object) {
    const topic = item as Topic;
    this.router.navigate([`/topics/${topic.id}/cards`]);
  }
}
