using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    public class Attempt
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<AttemptQuestion> Questions { get; set; }
    }

    public static class AttempExtensions
    {
        public static Attempt CreateNewAttempt(this Quiz quiz)
        {
            var attempt = new Attempt()
            {
                Id = Guid.NewGuid().ToString(),
                Name = quiz.Name,
                Questions = quiz.Questions.Select(q => new AttemptQuestion()
                {
                    Id = q.Id,
                    Question = q.Question,
                    Choices = q.Choices
                }).ToList()
            };
            return attempt;
        }
    }
}
