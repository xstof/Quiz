namespace Quiz.Model
{
    public interface IAttemptRepository
    {
        Attempt FindAttempt(string quizId, string attemptId);
        void StoreAttempt(string quizId, Attempt attempt);
    }
}