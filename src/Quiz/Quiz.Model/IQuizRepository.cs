namespace Quiz.Model
{
    public interface IQuizRepository
    {
        QuizQuestion AddQuestionToQuiz(string quizId, QuizQuestion question);
        Quiz CreateNewQuiz(Quiz quiz);
        Quiz FindQuiz(string id);
        void RemoveQuestionFromQuiz(string quizId, string questionId);
        void RemoveQuiz(string id);
    }
}