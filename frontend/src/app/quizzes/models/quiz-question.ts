import { CardSide } from "./card-side";

export interface QuizQuestion {
    id: string;
    topic: string;
    frontLabel: string;
    frontText: string;
    backLabel: string;
    backText: string;
    showFirst: CardSide;
}
