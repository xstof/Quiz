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
    public class AttemptController : ApiController
    {
        private readonly IQuizRepository quizRepo;
        private readonly IAttemptRepository attemptsRepo;

        public AttemptController(IQuizRepository quizRepo, IAttemptRepository attemptsRepo)
        {
            this.quizRepo = quizRepo;
            this.attemptsRepo = attemptsRepo;
        }

        [Route("api/quizes/{quizid}/attempts/{attemptid}", Name = "getattempt")]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK)]
        public HttpResponseMessage Get(string quizid, string attemptid)
        {
            var attempt = attemptsRepo.FindAttempt(quizid, attemptid);
            if(attempt == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }

            return Request.CreateResponse(HttpStatusCode.OK, attempt);
        }

        [Route("api/quizes/{quizid}/attempts")]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.Created)]
        public HttpResponseMessage Post(string quizid)
        {
            var quiz = quizRepo.FindQuiz(quizid);
            if (quiz == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }

            var attempt = quiz.CreateNewAttempt();
            attemptsRepo.StoreAttempt(quizid, attempt);

            var response = Request.CreateResponse(HttpStatusCode.OK, attempt);
            response.Headers.Location = new Uri(Request.RequestUri, Url.Route("getattempt", new {quizid = quizid, attemptid = attempt.Id }));
            return response;
        }

    }
}
