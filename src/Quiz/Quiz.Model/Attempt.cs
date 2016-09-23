using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    public class Attempt
    {
        public string QuizId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<AttemptQuestion> Questions { get; set; }
    }

    public static class AttemptExtensions
    {
        public static Attempt CreateNewAttempt(this Quiz quiz, string email)
        {
            var attempt = new Attempt()
            {
                QuizId = quiz.Id,
                Id = Guid.NewGuid().ToString(),
                Name = quiz.Name,
                Email = email,
                Questions = quiz.Questions.Select(q => new AttemptQuestion()
                {
                    Id = q.Id,
                    Question = q.Question,
                    Choices = q.Choices
                }).ToList()
            };
            return attempt;
        }

        public static AttemptScore Score(this Attempt attempt, List<AttemptAnswer> answers, IQuizRepository repo)
        {
            int nrCorrectAnswers = 0;
            int questionCount = attempt.Questions.Count();

            var quiz = repo.FindQuiz(attempt.QuizId);
            foreach(var question in quiz.Questions)
            {
                var questionAnswer = answers.FirstOrDefault(a => a.QuestionId == question.Id);
                if(questionAnswer != null && questionAnswer.Answer == question.CorrectAnswer)
                {
                    nrCorrectAnswers++;
                }
            }

            decimal percentageScore = ((decimal)nrCorrectAnswers / questionCount) * 100;

            return new AttemptScore()
            {
                NumberOfQuestions = questionCount,
                NumberOfCorrectAnswers = nrCorrectAnswers,
                ScoreInPercentage = (int)percentageScore
            };
        }
     }
}
