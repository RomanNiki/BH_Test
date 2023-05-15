using Factories;
using Mirror;

namespace Networking
{
    public class TestNetworkManager : NetworkManager
    {
        public new static TestNetworkManager singleton { get; private set; }
        private GameSystem _gameSystem;
        private bool _isGame;
        
        public override void Awake()
        {
            base.Awake();
            singleton = this;
        }

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
            base.OnStopServer();
            _isGame = false;
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            PlayerFactory.Instance.DeletePlayerSpawnInformation(conn.connectionId);
            base.OnServerDisconnect(conn);
        }
    }
}