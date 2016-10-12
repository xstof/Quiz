using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    public class PrepopulatedQuizRepositoryDecorator : IQuizRepository
    {
        private readonly IQuizRepository repo;

        public PrepopulatedQuizRepositoryDecorator(IQuizRepository repo)
        {
            this.repo = repo;

            prepopulate(repo);
        }

        private void prepopulate(IQuizRepository repo)
        {
            var quiz = repo.CreateNewQuiz(new Quiz()
            {
                Id = "0",
                Name = "CAD Demo Quiz"
            });

            repo.AddQuestionToQuiz(quiz.Id, new QuizQuestion()
            {
                Id = "0",
                Question = "What not true about a deployment slot?",
                Choices = new List<QuestionChoice>()
                 {
                     new QuestionChoice() { Choice = "A deployment slot provides the option to stage deployments before moving them in production." },
                     new QuestionChoice() { Choice = "A deployment slot allows to test in production by routing part of traffic to a different deployment." },
                     new QuestionChoice() { Choice = "A deployment slot allows an application to scale better." }
                 }, CorrectAnswer = 2
            });

            repo.AddQuestionToQuiz(quiz.Id, new QuizQuestion()
            {
                Id = "1",
                Question = "What continous deployment option is not supported?",
                Choices = new List<QuestionChoice>()
                 {
                     new QuestionChoice() { Choice = "GitHub" },
                     new QuestionChoice() { Choice = "Visual Studio Team Services" },
                     new QuestionChoice() { Choice = "BitBucket" },
                     new QuestionChoice() { Choice = "OneDrive" },
                     new QuestionChoice() { Choice = "DropBox" },
                     new QuestionChoice() { Choice = "Google Drive" }
                 }, CorrectAnswer = 5
            });

            repo.AddQuestionToQuiz(quiz.Id, new QuizQuestion()
            {
                Id = "2",
                Question = "What is not true about KUDU?",
                Choices = new List<QuestionChoice>()
                 {
                     new QuestionChoice() { Choice = "A Kudu is a south african animal?" },
                     new QuestionChoice() { Choice = "Kudu is the engine behind git deployments in Azure." },
                     new QuestionChoice() { Choice = "Kudu can be ran outside of Azure." },
                     new QuestionChoice() { Choice = "Kudu is open source" }
                 },
                CorrectAnswer = 0
            });

            repo.AddQuestionToQuiz(quiz.Id, new QuizQuestion()
            {
                Id = "3",
                Question = "What is local cache?",
                Choices = new List<QuestionChoice>()
                 {
                     new QuestionChoice() { Choice = "A feature that improves perf by allowing the use of the local disk in App Service." },
                     new QuestionChoice() { Choice = "Redis as a service in App Service." },
                     new QuestionChoice() { Choice = "An in-memory cache provider for App Service." },
                     new QuestionChoice() { Choice = "A way to track session state." }
                 },
                CorrectAnswer = 0
            });

            repo.AddQuestionToQuiz(quiz.Id, new QuizQuestion()
            {
                Id = "4",
                Question = "What is Easy Auth?",
                Choices = new List<QuestionChoice>()
                 {
                     new QuestionChoice() { Choice = "An easy way to configure authorization for web apps." },
                     new QuestionChoice() { Choice = "An easy way to configure authorization for api apps." },
                     new QuestionChoice() { Choice = "An easy way to configure authorization for both web and api apps." },
                     new QuestionChoice() { Choice = "Another name for OAuth in App Service." }
                 },
                CorrectAnswer = 2
            });
        }

        public QuizQuestion AddQuestionToQuiz(string quizId, QuizQuestion question)
        {
            return repo.AddQuestionToQuiz(quizId, question);
        }

        public IEnumerable<Quiz> AllQuizes()
        {
            return repo.AllQuizes();
        }

        public Quiz CreateNewQuiz(Quiz quiz)
        {
            return repo.CreateNewQuiz(quiz);
        }

        public Quiz FindQuiz(string id)
        {
            return repo.FindQuiz(id);
        }

        public IEnumerable<QuizQuestion> QuestionsForQuiz(string quizId)
        {
            return repo.QuestionsForQuiz(quizId);
        }

        public void RemoveQuestionFromQuiz(string quizId, string questionId)
        {
            repo.RemoveQuestionFromQuiz(quizId, questionId);
        }

        public void RemoveQuiz(string id)
        {
            repo.RemoveQuiz(id);
        }

        public QuizQuestion FindQuestion(string quizId, string questionId)
        {
            return repo.FindQuestion(quizId, questionId);
        }
    }
}
