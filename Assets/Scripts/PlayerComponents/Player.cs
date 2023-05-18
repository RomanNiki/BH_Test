using System;
using Mirror;
using Networking;
using UI;

namespace PlayerComponents
{
    public class Player : NetworkBehaviour
    {
        private GameSystem _gameSystem;

        [SyncVar(hook = nameof(OnNicknameChanged))]
        private string _nickname;
        public string Nickname => _nickname;
        [SyncVar(hook = nameof(OnScoreChanged))]
        private int _score;
        public event Action<int, string> ScoreChanged;
        public event Action<string> NicknameChanged;
        private ScoreBoardItem _playerScoreBoardItem;

        public override void OnStartLocalPlayer()
        {
            CmdUpdateScore();
        }

        public override void OnStartClient()
        {
            if (isLocalPlayer)
            {
                CmdSetNickname(PlayerSettings.Nickname);
            }
            if (ScoreBoard.Instance != null)
            {
                _playerScoreBoardItem = ScoreBoard.Instance.CreateScoreBoardItem(this);
            }
        }
        

        public override void OnStopLocalPlayer()
        {
            if (_gameSystem != null)
            {
                if (isServer)
                {
                    _gameSystem.RemovePlayer(this);
                }
                else
                {
                    _gameSystem.CmdRemovePlayer(this);
                }
            }

            if (ScoreBoard.Instance != null)
            {
                ScoreBoard.Instance.SetActive(false);
            }
         
            base.OnStopLocalPlayer();
        }

        public override void OnStopClient()
        {
            ScoreChanged = null;
            NicknameChanged = null;
            
            if (_playerScoreBoardItem != null)
            {
                Destroy(_playerScoreBoardItem.gameObject);
            }
        }

        private void OnNicknameChanged(string _, string newNickname)
        {
            NicknameChanged?.Invoke(newNickname);
        }

        private void OnScoreChanged(int _, int newScore)
        {
            ScoreChanged?.Invoke(newScore, _nickname);
        }

        [Command(requiresAuthority = false)]
        public void CmdSetNickname(string nickname)
        {
            SetNickname(nickname);
        }

        [Command(requiresAuthority = false)]
        private void CmdUpdateScore()
        {
            ScoreChanged?.Invoke(_score, _nickname);
        }

        [Server]
        public void SetNickname(string nickname)
        {
            _nickname = nickname;
        }

        [Command(requiresAuthority = false)]
        public void CmdIncreaseScore()
        {
            IncreaseScore();
        }

        [Server]
        private void IncreaseScore()
        {
            if (_gameSystem.HasWinner == false)
            {
                _score++;
            }
        }
        
        public void Init(GameSystem gameSystem)
        {
            _gameSystem = gameSystem;
            _gameSystem.AddPlayer(this);
        }
    }
}