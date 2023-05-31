using Interfaces;
using PlayerComponents;
using UnityEngine;

namespace Game
{
    public class ScoredGameSystem : BaseGameSystem
    {
        [SerializeField] private int _scoreToWin;
        private IWinCondition _winCondition;

        private void Awake()
        {
            _winCondition = new ScoreWinCondition(_scoreToWin);
        }

        private void OnPlayerScoreChanged(int score, string nickname)
        {
            if (HasWinner)
                return;
            if (_winCondition.CheckWinCondition(score) == false)
                return;

            HasWinner = true;
            EndGame(nickname);
        }

        protected override void OnPlayerAdd(Player player)
        {
            player.ScoreChanged += OnPlayerScoreChanged;
        }

        protected override void OnPlayerRemove(Player player)
        {
            player.ScoreChanged -= OnPlayerScoreChanged;
        }
    }
}