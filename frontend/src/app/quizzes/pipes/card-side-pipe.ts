import { Pipe, PipeTransform } from '@angular/core';
import { CardSide } from '@quizzes/models/card-side';

@Pipe({
  name: 'cardSide',
  standalone: false
})
export class CardSidePipe implements PipeTransform {

  transform(value: number): string {
    return CardSide[value];
  }

}
