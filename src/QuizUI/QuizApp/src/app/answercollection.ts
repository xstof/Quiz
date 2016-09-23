export class Answer {
    QuestionId: string;
    Answer: number;

    constructor(private questionId: string, private answer: number) {
        this.Answer = answer;
        this.QuestionId = questionId;
    }
}

export class AnswerCollection {
    Answers: Array<Answer>;

    constructor() {
        this.Answers = new Array<Answer>();
    }
}
