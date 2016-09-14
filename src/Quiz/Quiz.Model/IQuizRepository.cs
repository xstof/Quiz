using System.Collections.Generic;

namespace Quiz.Model
{
    public interface IQuizRepository
    {
        IEnumerable<Quiz> AllQuizes();
        QuizQuestion AddQuestionToQuiz(string quizId, QuizQuestion question);
        Quiz CreateNewQuiz(Quiz quiz);
        Quiz FindQuiz(string id);        
        void RemoveQuiz(string id);
        IEnumerable<QuizQuestion> QuestionsForQuiz(string quizId);
        QuizQuestion FindQuestion(string quizId, string questionId);
        void RemoveQuestionFromQuiz(string quizId, string questionId);
    }
}