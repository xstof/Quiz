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
    public class QuizzesController : ApiController
    {
        private readonly IQuizRepository repo;

        public QuizzesController(IQuizRepository repo)
        {
            this.repo = repo;
        }

        // GET api/quizes
        [SwaggerOperation("GetAllQuizzes")]
        [ResponseType(typeof(IEnumerable<Model.Quiz>))]
        public IEnumerable<Model.Quiz> Get()
        {
            return repo.AllQuizes();
        }

        // GET api/quizes/{quizid} 
        [SwaggerOperation("GetQuizById")]
        [SwaggerResponse(HttpStatusCode.OK, type:typeof(Quiz.Model.Quiz))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public HttpResponseMessage Get(string id)
        {
            var quiz = repo.FindQuiz(id);
            if(quiz == null)
            {   // Not found:                
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {   // Found:
                return Request.CreateResponse(HttpStatusCode.OK, quiz);
            }
        }

        // POST api/quizes
        [SwaggerOperation("CreateQuiz")]
        [SwaggerResponse(HttpStatusCode.Created, type: typeof(Quiz.Model.Quiz))]
        public HttpResponseMessage Post([FromBody]Model.Quiz quiz)
        {
            var newQuiz = repo.CreateNewQuiz(quiz);

            var response = Request.CreateResponse(HttpStatusCode.Created, newQuiz);
            response.Headers.Location = new Uri(Request.RequestUri, Url.Route("DefaultApi", new {controller="quizzes", id=quiz.Id }));
            return response;
        }

        // DELETE api/quizes/{quizid} 
        [SwaggerOperation("DeleteQuiz")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public HttpResponseMessage Delete(string id)
        {
            var quiz = repo.FindQuiz(id);
            if (quiz == null)
            {   // Not found:                
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {   // Found:
                repo.RemoveQuiz(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }        
    }
}
