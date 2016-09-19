export interface Choice {
    Choice: string;
}

export interface Question {
    Id: string;
    Question: string;
    Choices: Choice[];
}

export interface Attempt {
    QuizId: string;
    Id: string;
    Name: string;
    Email: string;
    Questions: Question[];
}