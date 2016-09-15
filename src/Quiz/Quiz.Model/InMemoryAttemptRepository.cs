using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    public class InMemoryAttemptRepository : IAttemptRepository
    {
        private Dictionary<string, Dictionary<string, Attempt>> attempts = 
            new Dictionary<string, Dictionary<string, Attempt>>();
        private Dictionary<string, Dictionary<string, AttemptScore>> scores =
            new Dictionary<string, Dictionary<string, AttemptScore>>();

        public void StoreAttempt(string quizId, Attempt attempt)
        {
            if (!attempts.ContainsKey(quizId)) { attempts.Add(quizId, new Dictionary<string, Attempt>()); }

            attempts[quizId].Add(attempt.Id, attempt);
        }

        public Attempt FindAttempt(string quizId, string attemptId)
        {
            if (string.IsNullOrWhiteSpace(quizId)) { throw new ArgumentNullException("quizId");  }
            if (string.IsNullOrWhiteSpace(attemptId)) { throw new ArgumentNullException("attemptId"); }
            if (!attempts.ContainsKey(quizId)) { return null; }
            if (!attempts[quizId].ContainsKey(attemptId)) { return null; }

            return attempts[quizId][attemptId];
        }

        public void StoreAttemptScore(string quizId, string attemptId, AttemptScore score)
        {
            var attempt = FindAttempt(quizId, attemptId);
            if(attempt == null) { throw new InvalidOperationException("Cannot store score for attempt which does not exist."); }

            if (!scores.ContainsKey(quizId)) { scores.Add(quizId, new Dictionary<string, AttemptScore>()); }

            scores[quizId][attemptId] = score;
        }

        public AttemptScore FindScore(string quizId, string attemptId)
        {
            if (string.IsNullOrWhiteSpace(quizId)) { throw new ArgumentNullException("quizId"); }
            if (string.IsNullOrWhiteSpace(attemptId)) { throw new ArgumentNullException("attemptId"); }
            if (!scores.ContainsKey(quizId)) { return null; }
            if (!scores[quizId].ContainsKey(attemptId)) { return null; }

            return scores[quizId][attemptId];
        }
    }
}
