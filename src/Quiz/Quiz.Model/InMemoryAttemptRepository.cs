using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    public class InMemoryAttemptRepository : IAttemptRepository
    {
        private Dictionary<string, Dictionary<string, Attempt>> attempts = new Dictionary<string, Dictionary<string, Attempt>>();

        public void StoreAttempt(string quizId, Attempt attempt)
        {
            if (!attempts.ContainsKey(quizId)) { attempts.Add(quizId, new Dictionary<string, Attempt>()); }

            attempts[quizId].Add(attempt.Id, attempt);
        }

        public Attempt FindAttempt(string quizId, string attemptId)
        {
            if (!attempts.ContainsKey(quizId)) { return null; }
            if (!attempts[quizId].ContainsKey(attemptId)) { return null; }

            return attempts[quizId][attemptId];
        }
    }
}
