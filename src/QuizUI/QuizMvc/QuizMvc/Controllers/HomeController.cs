using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuizMvc.QuizApi;
using AutoMapper;

namespace QuizMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuizAPIClient quizClient;
        private readonly IMapper mapper;

        public HomeController(IQuizAPIClient quizClient, IMapper mapper)
        {
            this.quizClient = quizClient;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            // fetch quizzes:
            var availableQuizzes = quizClient.GetAllQuizzes();
      
            // return overview from which quiz can be chosen:
            return View(availableQuizzes);
        }

        public IActionResult Quiz(string id)
        {
            // fetch quiz:
            var attempt = quizClient.StartAttempt(id, new QuizApi.Models.AttemptRequest("no.email@noemail.com"));
            var mappedAttempt = mapper.Map<Models.QuizAnswer>(attempt);

            return View(mappedAttempt);
        }

        public IActionResult Score(Models.QuizAnswer quizAnswer)
        {

            var scoreReq = mapper.Map<QuizApi.Models.ScoreRequest>(quizAnswer);

            var score = quizClient.CalculateAttemptScore(quizAnswer.QuizId,
                                                         quizAnswer.AttemptId,
                                                         scoreReq);

            return View(score);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
