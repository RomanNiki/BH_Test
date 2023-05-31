using Factories;
using Game;
using Mirror;

namespace Networking
{
    public class TestNetworkManager : NetworkManager
    {
        private GameSystem _gameSystem;
        private bool _isGame;
        private PlayerFactory _playerFactory;

        public override void OnServerSceneChanged(string sceneName)
        {
            base.OnServerSceneChanged(sceneName);
            FindGameSceneReferences(sceneName);
        }

        private void FindGameSceneReferences(string sceneName)
        {
            if (sceneName.Contains(Scenes.Game) == false) return;
            _gameSystem = FindObjectOfType<GameSystem>();
            _playerFactory = new PlayerFactory(_gameSystem);
            _playerFactory.LoadSpawnPoints(startPositions);
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            if (_playerFactory == null)
                return;
            _playerFactory.CreatePlayer(conn);
            if (numPlayers < 2 || _isGame) return;
            _isGame = true;
            _gameSystem.StartGame();
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            if (numPlayers - 1 < 2)
            {
                _isGame = false;
            }
            if (_playerFactory == null)
                return;
            _playerFactory.DeletePlayerSpawnInformation(conn.connectionId);
           
            base.OnServerDisconnect(conn);
        }
    }
}