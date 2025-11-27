import { CardSide } from "./card-side";

export interface Quiz {
    id: string;
    topicId?: string;
    topicName: string;
    showFirst: CardSide;
    isPracticeMode: boolean;
    numberOfQuestions: number;
    numberOfAnsweredQuestions?: number | null;
    numberOfCorrectAnswers?: number | null;
}
