using Quiz.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiz.API.WireModels
{
    public class ScoreRequest
    {
        public List<AttemptAnswer> Answers { get; set; }
    }
}