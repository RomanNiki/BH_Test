using PlayerComponents;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreBoardItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _usernameText;
        [SerializeField] private TMP_Text _scoreText;
        private Player _player;
        
        public void Init(Player player)
        {
            _player = player;
            _usernameText.text = _player.Nickname;
            _player.ScoreChanged += OnScoreChanged;
            _player.NicknameChanged += OnNicknameChanged;
        }

        private void OnScoreChanged(int score, string nickname)
        {
            OnNicknameChanged(nickname);
            _scoreText.text = score.ToString();
        }  
        
        private void OnNicknameChanged(string nickname)
        {
            _usernameText.text = nickname;
        }
    }
}