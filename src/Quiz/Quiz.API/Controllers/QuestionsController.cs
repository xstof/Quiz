using Quiz.Model;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
        [SwaggerResponse(HttpStatusCode.OK)]
        [Route("quizes/{quizid}/questions")]
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

        // POST api/quizes/{quizid}/questions
    }
}
