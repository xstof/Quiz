using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    public class AttemptScore
    {
        public int NumberOfQuestions { get; set; }
        public int NumberOfCorrectAnswers { get; set; }
        public int ScoreInPercentage { get; set; }
    }
}
