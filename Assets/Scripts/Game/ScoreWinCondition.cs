using Interfaces;

namespace Game
{
    public class ScoreWinCondition : IWinCondition
    {
        private readonly int _scoreToWin;

        public ScoreWinCondition(int scoreToWin)
        {
            _scoreToWin = scoreToWin;
        }

        public bool CheckWinCondition(int score)
        {
            return score >= _scoreToWin;
        }
    }
}