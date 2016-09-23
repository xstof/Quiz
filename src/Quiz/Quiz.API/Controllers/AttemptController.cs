﻿using Quiz.API.WireModels;
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
      
        [SwaggerOperation("GetAttempt")]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK)]
        [Route("api/quizes/{quizid}/attempts/{attemptid}", Name = "getattempt")]
        public HttpResponseMessage Get(string quizid, string attemptid)
        {
            var attempt = attemptsRepo.FindAttempt(quizid, attemptid);
            if(attempt == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }

            return Request.CreateResponse(HttpStatusCode.OK, attempt);
        }
        
        [SwaggerOperation("StartAttempt")]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [Route("api/quizes/{quizid}/attempts")]
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
        [SwaggerResponse(HttpStatusCode.OK)]
        [Route("api/quizes/{quizid}/attempts/{attemptid}/score")]
        public HttpResponseMessage PostScore(string quizid, string attemptid, [FromBody] ScoreRequest scoreReq)
        {
            var quiz = quizRepo.FindQuiz(quizid);
            if (quiz == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }
            var attempt = attemptsRepo.FindAttempt(quizid, attemptid);
            if (attempt == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }
            if (scoreReq == null) { return Request.CreateResponse(HttpStatusCode.BadRequest); }

            var score = attempt.Score(scoreReq.Answers, quizRepo);
            attemptsRepo.StoreAttemptScore(quizid, attemptid, score);

            return Request.CreateResponse(HttpStatusCode.OK, score);
        }

        [SwaggerOperation("GetAttemptScore")]        
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK)]
        [Route("api/quizes/{quizid}/attempts/{attemptid}/score")]
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
    }
}
