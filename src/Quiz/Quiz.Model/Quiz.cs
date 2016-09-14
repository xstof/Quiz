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
            this.Questions = new Dictionary<string, QuizQuestion>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, QuizQuestion> Questions { get; set; }
    }
}
