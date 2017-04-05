// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace QuizMvc.QuizApi.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;

    public partial class QuizQuestion
    {
        /// <summary>
        /// Initializes a new instance of the QuizQuestion class.
        /// </summary>
        public QuizQuestion() { }

        /// <summary>
        /// Initializes a new instance of the QuizQuestion class.
        /// </summary>
        public QuizQuestion(string id = default(string), string question = default(string), IList<QuestionChoice> choices = default(IList<QuestionChoice>), int? correctAnswer = default(int?))
        {
            Id = id;
            Question = question;
            Choices = choices;
            CorrectAnswer = correctAnswer;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Question")]
        public string Question { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Choices")]
        public IList<QuestionChoice> Choices { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CorrectAnswer")]
        public int? CorrectAnswer { get; set; }

    }
}
