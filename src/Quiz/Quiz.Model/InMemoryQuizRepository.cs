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

        public IEnumerable<Quiz> AllQuizes()
        {
            return quizes.Values.ToList();
        }

        public Quiz CreateNewQuiz(Quiz quiz)
        {
            var quizId = quiz.EnsureId();
            quizes.Add(quizId, quiz);

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
            quizes[quizId].Questions.Add(question);
            return question;
        }

        public void RemoveQuestionFromQuiz(string quizId, string questionId)
        {
            if (!quizes.ContainsKey(quizId))
            {
                throw new InvalidOperationException("Cannot remove question from quiz that doesnt exist.");
            }
            if (!quizes[quizId].Questions.Any(q => q.Id == questionId))
            {
                throw new InvalidOperationException("Cannot find question to remove.");
            }
            var question = quizes[quizId].Questions.First(q => q.Id == questionId);
            quizes[quizId].Questions.Remove(question);
        }

        public Quiz FindQuiz(string id)
        {
            if (!quizes.ContainsKey(id)) { return null; }

            return quizes[id];
        }

        public IEnumerable<QuizQuestion> QuestionsForQuiz(string quizId)
        {
            if (!quizes.ContainsKey(quizId))
            {
                throw new InvalidOperationException("Cannot retrieve questions from quiz that doesnt exist.");
            }

            return quizes[quizId].Questions;
        }

        public QuizQuestion FindQuestion(string quizId, string questionId)
        {
            if (!quizes.ContainsKey(quizId))
            {
                throw new InvalidOperationException("Cannot retrieve question from quiz that doesnt exist.");
            }

            return quizes[quizId].Questions.FirstOrDefault(q => q.Id == questionId);
        }
    }
}
