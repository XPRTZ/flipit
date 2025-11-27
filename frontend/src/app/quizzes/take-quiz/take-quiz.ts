import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QuizQuestion } from '@quizzes/models/quiz-question';
import { Quiz } from '@quizzes/models/quiz';
import { QuizService } from '@quizzes/services/quiz-service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-take-quiz',
  standalone: false,
  templateUrl: './take-quiz.html',
  styleUrl: './take-quiz.sass',
})
export class TakeQuiz {
  private route = inject(ActivatedRoute);
  private quizService = inject(QuizService);

  quizId = this.route.snapshot.paramMap.get('quizId')!;
  quiz = signal<Quiz | null>(null);
  question = signal<QuizQuestion | null>(null);

  ngOnInit() {
    this.loadQuizAndQuestion();
  }

  loadQuizAndQuestion() {
    let loadQuiz = this.quizService.getQuiz(this.quizId);
    let loadQuestion = this.quizService.getNextQuestion(this.quizId);

    forkJoin({ quiz: loadQuiz, question: loadQuestion })
      .subscribe(({ quiz, question }) => {
        this.quiz.set(quiz);
        this.question.set(question);
      });
  }

  answerQuestion(isCorrect: boolean) {
    this.quizService.answerQuestion(this.quizId, this.question()!.id, isCorrect)
      .subscribe(() => {
        this.loadQuizAndQuestion();
      });
  }

  getCommentary() : string {
    if (this.quiz() === null) {
      return '';
    }
    const ratio = (this.quiz()!.numberOfCorrectAnswers ?? 0) / this.quiz()!.numberOfQuestions;
    if (ratio <= 0.2) {
      return 'Keep at it, you\'ll get there!';
    } else if (ratio <= 0.4) {
      return 'Could be better, but you\'re improving!';
    } else if (ratio <= 0.6) {
      return 'Not bad! Keep it up!';
    } else if (ratio <= 0.8) {
      return 'Good job, you\'re doing great!';
    } else {
      return 'Amazing! Great job!'
    }
  }
}
