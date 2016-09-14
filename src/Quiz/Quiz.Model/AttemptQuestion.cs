using System.Collections.Generic;

namespace Quiz.Model
{
    public class AttemptQuestion
    {
        public string Id { get; set; }
        public string Question { get; set; }
        public List<QuestionChoice> Choices { get; set; }
    }
}