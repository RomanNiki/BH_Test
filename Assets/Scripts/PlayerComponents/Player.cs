using System;
using Mirror;
using Networking;

namespace PlayerComponents
{
    public class Player : NetworkBehaviour
    {
        private GameSystem _gameSystem;

        [SyncVar(hook = nameof(OnNicknameChanged))]
        private string _nickname;

        [SyncVar(hook = nameof(OnScoreChanged))]
        private int _score;

        public event Action<int, string> ScoreChanged;
        public event Action<string> NicknameChanged;

        private void Start()
        {
            if (isLocalPlayer)
            {
                CmdSetNickname(PlayerSettings.Nickname);
            }
        }

        public override void OnStopLocalPlayer()
        {
            if (_gameSystem != null)
            {
                _gameSystem.CmdRemovePlayer(this);
            }

            base.OnStopLocalPlayer();
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
            _score++;
        }

        public void Init(GameSystem gameSystem)
        {
            _gameSystem = gameSystem;
            _gameSystem.AddPlayer(this);
        }
    }
}