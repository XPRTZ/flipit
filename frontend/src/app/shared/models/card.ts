export interface Card {
    id: string;
    front: string;
    back: string;
    correctnessRatio: number;
    lastViewedAt: string | null;
}
