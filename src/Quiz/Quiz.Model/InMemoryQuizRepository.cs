using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    public class InMemoryQuizRepository : IQuizRepository
    {
        private static Dictionary<string, Quiz> quizes = new Dictionary<string, Quiz>();

        public Quiz CreateNewQuiz(Quiz quiz)
        {
            if (String.IsNullOrWhiteSpace(quiz.Id)) { quiz.Id = System.Guid.NewGuid().ToString(); }
            quizes.Add(quiz.Id, quiz);

            return quiz;
        }

        public void RemoveQuiz(string id)
        {
            if (!quizes.ContainsKey(id))
            {
                throw new InvalidOperationException("Cannot remove quiz that doesnt exist.");
            }
            quizes.Remove(id);
        }

        public QuizQuestion AddQuestionToQuiz(string quizId, QuizQuestion question)
        {
            if (!quizes.ContainsKey(quizId))
            {
                throw new InvalidOperationException("Cannot add question to quiz that doesnt exist.");
            }

            var questionId = question.EnsureId();
            quizes[quizId].Questions.Add(questionId, question);
            return question;
        }

        public void RemoveQuestionFromQuiz(string quizId, string questionId)
        {
            if (!quizes.ContainsKey(quizId))
            {
                throw new InvalidOperationException("Cannot remove question from quiz that doesnt exist.");
            }
            if (!quizes[quizId].Questions.ContainsKey(questionId))
            {
                throw new InvalidOperationException("Cannot find question to remove.");
            }
        }

        public Quiz FindQuiz(string id)
        {
            if (!quizes.ContainsKey(id)) { return null; }

            return quizes[id];
        }

       
    }
}
