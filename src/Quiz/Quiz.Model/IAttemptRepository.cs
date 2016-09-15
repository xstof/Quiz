namespace Quiz.Model
{
    public interface IAttemptRepository
    {
        Attempt FindAttempt(string quizId, string attemptId);
        void StoreAttempt(string quizId, Attempt attempt);
        void StoreAttemptScore(string quizId, string attemptId, AttemptScore score);
        AttemptScore FindScore(string quizId, string attemptId);
    }
}