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
    public class QuizController : ApiController
    {
        private readonly IQuizRepository repo;

        public QuizController(IQuizRepository repo)
        {
            this.repo = repo;
        }

        // GET api/quiz
        [SwaggerOperation("GetAllQuizes")]
        public IEnumerable<Model.Quiz> Get()
        {
            return repo.AllQuizes();
        }
    }
}
