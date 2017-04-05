using Quiz.Model;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Quiz.API.Controllers
{
    public class QuestionsController : ApiController
    {
        private readonly IQuizRepository repo;
        public QuestionsController(IQuizRepository repo)
        {
            this.repo = repo;
        }

        // GET api/quizes/{quizid}/questions
        [SwaggerOperation("GetAllQuizQuestions")]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(IEnumerable<QuizQuestion>))]
        [Route("api/quizzes/{quizid}/questions")]
        public HttpResponseMessage Get(string quizid)
        {
            var quiz = repo.FindQuiz(quizid);
            if (quiz == null)
            {   // Not found:                
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {   // Found:
                var questions = repo.QuestionsForQuiz(quizid);
                var response = Request.CreateResponse(HttpStatusCode.OK, questions);
                return response;
            }
        }

        // GET api/quizes/{quizid}/questions/{questionid}
        [SwaggerOperation("GetQuizQuestionById")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(QuizQuestion))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("api/quizzes/{quizid}/questions/{questionid}", Name = "getsinglequestion")]
        public HttpResponseMessage Get(string quizid, string questionid)
        {
            var quiz = repo.FindQuiz(quizid);
            if (quiz == null)
            {   // Quiz not found:                
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            var question = repo.FindQuestion(quizid, questionid);

            if (question == null)
            {   // Question not found:
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            
            return Request.CreateResponse(HttpStatusCode.OK, question);
        }

        // POST api/quizes/{quizid}/questions
        [SwaggerOperation("AddQuizQuestion")]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.Created)]
        [Route("api/quizzes/{quizid}/questions")]
        public HttpResponseMessage Post(string quizid, [FromBody] Model.QuizQuestion question)
        {
            var quiz = repo.FindQuiz(quizid);
            if (quiz == null)
            {   // Not found:                
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {   // Found:
                var questions = repo.AddQuestionToQuiz(quizid, question);
                var response = Request.CreateResponse(HttpStatusCode.Created);
                response.Headers.Location = new Uri(Request.RequestUri, Url.Route("getsinglequestion", new { questionid = question.Id }));
                return response;
            }
        }

        // DELETE api/quizes/{quizid}/questions/{questionid}
        [SwaggerOperation("DeleteQuizQuestion")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("api/quizzes/{quizid}/questions/{questionid}")]
        public HttpResponseMessage Delete(string quizid, string questionid)
        {
            var quiz = repo.FindQuiz(quizid);
            if (quiz == null)
            {   // Quiz not found:                
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }            
            else if(repo.FindQuestion(quizid, questionid) == null)
            {   // Question not found:
                return Request.CreateResponse(HttpStatusCode.NotFound);                
            }

            repo.RemoveQuestionFromQuiz(quizid, questionid);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
