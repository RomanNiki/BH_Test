using System.Collections.Generic;
using System.Linq;
using Game;
using Mirror;
using PlayerComponents;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Factories
{
    public class PlayerFactory
    {
        private readonly GameSystem _gameSystem;
        private readonly List<SpawnPointData> _spawnPoints;

        public PlayerFactory(GameSystem gameSystem)
        {
            _gameSystem = gameSystem;
            _spawnPoints = new List<SpawnPointData>();
        }

        public void LoadSpawnPoints(IEnumerable<Transform> startPositions)
        {
            foreach (var startPosition in startPositions)
            {
                _spawnPoints.Add(new SpawnPointData(startPosition));
            }
        }

        public void DeletePlayerSpawnInformation(int connectionId)
        {
            var spawnPointData = _spawnPoints.FirstOrDefault(x => x.ConnectionId == connectionId);
            if (spawnPointData != null)
            {
                spawnPointData.ConnectionId = null;
            }
        }
        
        private Transform ReservePosition(int connectionId)
        {
            var spawnPointDatas = _spawnPoints.Where(x => x.ConnectionId == null).ToList();

            if (spawnPointDatas.Count == 0)
            {
                return null;
            }

            var spawnPointData = spawnPointDatas[Random.Range(0, spawnPointDatas.Count)];

            spawnPointData.ConnectionId = connectionId;
            return spawnPointData.Transform;
        }
        

        public Player CreatePlayer(NetworkConnectionToClient connection)
        {
            var spawnTransform = ReservePosition(connection.connectionId);
            if (spawnTransform == null)
            {
                return null;
            }

            var playerGameObject = Object.Instantiate(NetworkManager.singleton.playerPrefab, spawnTransform.position,
                Quaternion.identity);
            var player = playerGameObject.GetComponent<Player>();
            player.Init(_gameSystem);
            NetworkServer.AddPlayerForConnection(connection, player.gameObject);
            return player;
        }
    }

    public class SpawnPointData
    {
        public Transform Transform { get; }
        public int? ConnectionId { get; set; }

        public SpawnPointData(Transform transform)
        {
            Transform = transform;
            ConnectionId = null;
        }
    }
}