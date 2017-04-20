using Microsoft.AspNet.WebHooks;
using Quiz.API.WireModels;
using Quiz.Model;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

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
      
        [SwaggerOperation("GetAttempt")]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK, type:typeof(Model.Attempt))]
        [Route("api/quizzes/{quizid}/attempts/{attemptid}", Name = "getattempt")]
        public HttpResponseMessage Get(string quizid, string attemptid)
        {
            var attempt = attemptsRepo.FindAttempt(quizid, attemptid);
            if(attempt == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }

            return Request.CreateResponse(HttpStatusCode.OK, attempt);
        }
        
        [SwaggerOperation("StartAttempt")]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(Quiz.Model.Attempt))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [Route("api/quizzes/{quizid}/attempts")]
        public HttpResponseMessage Post(string quizid, [FromBody] AttemptRequest attemptReq)
        {
            var quiz = quizRepo.FindQuiz(quizid);
            if (quiz == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }
            if (attemptReq == null || !attemptReq.IsValid()) { return Request.CreateResponse(HttpStatusCode.BadRequest); }

            var attempt = quiz.CreateNewAttempt(attemptReq.Email);
            attemptsRepo.StoreAttempt(quizid, attempt);

            var response = Request.CreateResponse(HttpStatusCode.OK, attempt);
            response.Headers.Location = new Uri(Request.RequestUri, Url.Route("getattempt", new {quizid = quizid, attemptid = attempt.Id }));
            return response;
        }

        [SwaggerOperation("CalculateAttemptScore")]   
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.OK, type:typeof(AttemptScore))]
        [Route("api/quizzes/{quizid}/attempts/{attemptid}/score")]
        public async Task<HttpResponseMessage> PostScore(string quizid, string attemptid, [FromBody] ScoreRequest scoreReq)
        {
            var quiz = quizRepo.FindQuiz(quizid);
            if (quiz == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }
            var attempt = attemptsRepo.FindAttempt(quizid, attemptid);
            if (attempt == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }
            if (scoreReq == null) { return Request.CreateResponse(HttpStatusCode.BadRequest); }

            // Calculate score for the given attempt and store this:
            var score = attempt.Score(scoreReq.Answers, quizRepo);
            attemptsRepo.StoreAttemptScore(quizid, attemptid, score);

            // Raise webhook when score was posted and recorded:
            await this.NotifyAllAsync("attemptscored", new { quizid = quizid, attemptid = attemptid });
            //await this._manager.NotifyAllAsync("attemptscored", new { QuizId = quizid, AttemptId = attemptid });


            return Request.CreateResponse(HttpStatusCode.OK, score);
        }

        [SwaggerOperation("GetAttemptScore")]        
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(AttemptScore))]

        [Route("api/quizzes/{quizid}/attempts/{attemptid}/score")]
        public HttpResponseMessage GetScore(string quizid, string attemptid)
        {
            var quiz = quizRepo.FindQuiz(quizid);
            if (quiz == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }
            var attempt = attemptsRepo.FindAttempt(quizid, attemptid);
            if (attempt == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }

            var score = attemptsRepo.FindScore(quizid, attemptid);
            if(score == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }

            return Request.CreateResponse(HttpStatusCode.OK, score);
        }


        // Webhooks support, the DIY way for demo-purposes (no authentication here): 
        private Microsoft.AspNet.WebHooks.IWebHookManager _manager;
        private Microsoft.AspNet.WebHooks.IWebHookStore _store;
        private Microsoft.AspNet.WebHooks.IWebHookUser _user;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);

            _manager = Configuration.DependencyResolver.GetManager();
            _store = Configuration.DependencyResolver.GetStore();
            _user = Configuration.DependencyResolver.GetUser();

            //// Set up default WebHook logger
            //var logger = new Microsoft.AspNet.WebHooks.Diagnostics.TraceLogger();

            //// Set the WebHook Store we want to get WebHook subscriptions from. Azure store requires
            //// a valid Azure Storage connection string named MS_AzureStoreConnectionString.
            //IWebHookStore store = new MemoryWebHookStore();

            //// Set the sender we want to actually send out the WebHooks. We could also 
            //// enqueue messages for scale out.
            //IWebHookSender sender = new DataflowWebHookSender(logger);

            //// Set up WebHook manager which we use for creating notifications.
            //this._manager = new WebHookManager(store, sender, logger);
        }
    }
}
