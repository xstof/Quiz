using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMvc.Models
{
    public class QuizAnswer
    {
        [HiddenInput]
        public string QuizName { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string QuizId { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string AttemptId { get; set; }
        public List<QuizQuestionAnswer> Questions { get; set; }
    }

    public class QuizQuestionAnswer
    {
        [HiddenInput]
        public string QuestionText { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string QuestionId { get; set; }
        public string SelectedOptionId { get; set; }
        public IList<QuizQuestionOption> Options { get; set; }
    }

    public class QuizQuestionOption
    {
        public string OptionText { get; set; }
        public string OptionId { get; set; }
    }
}
