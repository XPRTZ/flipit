import { Component, inject, signal } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CardSide } from '@quizzes/models/card-side';
import { Quiz } from '@quizzes/models/quiz';
import { QuizService } from '@quizzes/services/quiz-service';
import { ItemColumn } from '@shared/models/item-column';
import { Topic } from '@shared/models/topic';
import { CardService } from '@shared/services/card-service';
import { TopicService } from '@shared/services/topic-service';
import { catchError, first } from 'rxjs';

@Component({
  selector: 'app-create-quiz',
  standalone: false,
  templateUrl: './create-quiz.html',
  styleUrl: './create-quiz.sass',
})
export class CreateQuiz {
  private router = inject(Router);
  private topicService = inject(TopicService);
  private cardService = inject(CardService);
  private quizService = inject(QuizService);

  quizzes = signal<Quiz[]>([]);

  topics = signal<Topic[]>([]);
  topic = new FormControl<Topic | null>(null, Validators.required);
  frontLabel = signal<string>('');
  backLabel = signal<string>('');

  numberOfCards = signal<number>(1);
  numberOfQuestions = new FormControl<number>(1,
    [Validators.required, Validators.min(1), Validators.max(this.numberOfCards())]);

  readonly form = new FormBuilder().group({
    mode: ['quiz', Validators.required],
    side: ['front', Validators.required],
    topic: this.topic,
    numberOfQuestions: this.numberOfQuestions,
  });

  ngOnInit() {
    this.loadQuizzes();
    this.loadTopics();
  }

  getColumns(): ItemColumn[] {
    return [
      { name: 'topicName', label: 'Topic Name', editable: false },
      { name: 'showFirst', label: 'Show First', editable: false },
      { name: 'isPracticeMode', label: 'Practice Mode?', editable: false },
      { name: 'numberOfQuestions', label: 'Number of Questions', editable: false },
      { name: 'numberOfAnsweredQuestions', label: 'Questions Answered', editable: false },
      { name: 'numberOfCorrectAnswers', label: 'Correct Answers', editable: false }
    ];
  }

  loadQuizzes() {
    this.quizService
      .getQuizzes()
      .pipe(
        catchError(err => {
          this.quizzes.set([]);
          throw err;
        })
      )
      .subscribe(quizzes => {
        this.quizzes.set(quizzes);
      });
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
        if (this.topic.value === null && topics.length > 0) {
          this.topic.setValue(topics[0]);
          this.onTopicChange();
        }
      });
  }

  loadCards(topicId: string) {
    this.cardService
      .getCards(topicId)
      .pipe(
        first()
      )
      .subscribe(cards => {
        const maxQuestions = Math.max(cards.length, 1);
        this.numberOfQuestions.setValidators(
          [Validators.required, Validators.min(1), Validators.max(maxQuestions)]
        );
        this.numberOfCards.set(maxQuestions);
      });
  }

  onTopicChange(): void {
    const topic = this.topic.value;
    if (topic) {
      this.loadCards(topic.id);
      this.frontLabel.set(topic.frontLabel);
      this.backLabel.set(topic.backLabel);
    }
  }

  onSubmit(): void {
    if (!this.form.valid) {
      return;
    }

    const topic = this.topic.value!;
    const numberOfQuestions = this.numberOfQuestions.value!;
    const side = this.form.get('side')?.value === 'front' ? CardSide.Front : CardSide.Back;
    const isPracticeMode = this.form.get('mode')?.value === 'practice';

    this.quizService.createQuiz({
      id: '',
      topicId: topic.id,
      topicName: topic.name,
      numberOfQuestions: numberOfQuestions,
      showFirst: side,
      isPracticeMode: isPracticeMode
    })
      .subscribe(quiz => {
        this.router.navigate([`/quizzes/${quiz.id}`]);
      });
  }

  goToQuiz(item: object) {
    const quiz = item as Quiz;
    this.router.navigate([`/quizzes/${quiz.id}`]);
  }
}
