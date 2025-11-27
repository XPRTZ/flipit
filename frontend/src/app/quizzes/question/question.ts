import { Component, effect, input, output, signal } from '@angular/core';
import { CardSide } from '@quizzes/models/card-side';
import { Quiz } from '@quizzes/models/quiz';
import { QuizQuestion } from '@quizzes/models/quiz-question';

@Component({
  selector: 'app-question',
  standalone: false,
  templateUrl: './question.html',
  styleUrl: './question.sass',
})
export class Question {
  quiz = input.required<Quiz>();
  question = input.required<QuizQuestion>();

  answer = output<boolean>();

  isAnswered = signal<boolean>(false);
  currentSide = signal<CardSide>(CardSide.Front);
  isQuestionChanging = signal<boolean>(false);
  lastQuestion : QuizQuestion | null = null;

  constructor() {
    effect(() => {
      if (this.question() !== this.lastQuestion) {
        this.isQuestionChanging.set(false);
      }
    })
  }

  ngOnInit() {
    this.currentSide.set(this.quiz().showFirst);
  }

  showFront() : boolean {
    return this.currentSide() === CardSide.Front;
  }

  showBack() : boolean {
    return this.currentSide() === CardSide.Back;
  }

  flipCard() {
    const newSide = this.currentSide() === CardSide.Front ? CardSide.Back : CardSide.Front;
    this.currentSide.set(newSide);

    if (!this.isAnswered()) {
      this.isAnswered.set(true);
    }
  }

  getQuizProgress() : string {
    const questions = this.quiz().numberOfQuestions;
    const currentQuestion = (this.quiz().numberOfAnsweredQuestions ?? 0) + 1;
    return `${currentQuestion} / ${questions}`;
  }

  onAnswer(isCorrect: boolean) {
    this.isQuestionChanging.set(true);
    this.lastQuestion = this.question();

    this.answer.emit(isCorrect);

    this.isAnswered = signal<boolean>(false);
    this.currentSide.set(this.quiz().showFirst);
  }
}
