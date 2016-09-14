using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    public class QuizQuestion
    {
        public string Id { get; set; }
        public string Question { get; set; }
        public List<QuestionChoice> Choices {get; set;} 
        public int CorrectAnswer { get; set; }
    }

    public static class QuizQuestionExtensions
    {
        public static string EnsureId(this QuizQuestion question)
        {
            if (String.IsNullOrWhiteSpace(question.Id))
            {
                question.Id = Guid.NewGuid().ToString();
            }

            return question.Id;
        }
    }
}
