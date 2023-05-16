using Factories;
using Mirror;

namespace Networking
{
    public class TestNetworkManager : NetworkManager
    {
        private GameSystem _gameSystem;
        private bool _isGame;

        public override void OnServerSceneChanged(string sceneName)
        {
            base.OnServerSceneChanged(sceneName);
            _gameSystem = FindObjectOfType<GameSystem>();
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            var player = PlayerFactory.Instance.CreatePlayer(conn.connectionId);
            NetworkServer.AddPlayerForConnection(conn, player);
            if (numPlayers == 2 && _isGame == false)
            {
                _isGame = true;
                _gameSystem.StartGame();
            }
        }

        public override void OnStopServer()
        {
            _isGame = false;
            base.OnStopServer();
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            PlayerFactory.Instance.DeletePlayerSpawnInformation(conn.connectionId);
            base.OnServerDisconnect(conn);
        }
    }
}