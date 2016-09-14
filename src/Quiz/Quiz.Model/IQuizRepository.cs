using System.Collections.Generic;

namespace Quiz.Model
{
    public interface IQuizRepository
    {
        IEnumerable<Quiz> AllQuizes();
        QuizQuestion AddQuestionToQuiz(string quizId, QuizQuestion question);
        Quiz CreateNewQuiz(Quiz quiz);
        Quiz FindQuiz(string id);
        void RemoveQuestionFromQuiz(string quizId, string questionId);
        void RemoveQuiz(string id);
    }
}