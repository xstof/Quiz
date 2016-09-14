using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    public class Quiz
    {
        public Quiz()
        {
            this.Questions = new List<QuizQuestion>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public List<QuizQuestion> Questions { get; set; }
    }

    public static class QuizExtensions
    {
        public static string EnsureId(this Quiz quiz)
        {
            if (String.IsNullOrWhiteSpace(quiz.Id))
            {
                quiz.Id = Guid.NewGuid().ToString();
            }
            return quiz.Id;
        }
    }
}
