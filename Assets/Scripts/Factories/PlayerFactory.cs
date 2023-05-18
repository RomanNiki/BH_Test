using System.Collections.Generic;
using System.Linq;
using Mirror;
using PlayerComponents;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Factories
{
    public class PlayerFactory : MonoBehaviour
    {
        [SerializeField] private GameSystem _gameSystem;
        private List<SpawnPointData> _spawnPoints;
        public static PlayerFactory Instance { get; private set; }

        private void Awake()
        {
            if (Instance is null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void OnDestroy()
        {
            if (this == Instance)
            {
                Instance = null;
            }
        }

        private void OnEnable()
        {
            LoadSpawnPoints();
        }

        private void LoadSpawnPoints()
        {
            var startPositions = NetworkManager.startPositions;
            _spawnPoints = new List<SpawnPointData>();

            foreach (var startPosition in startPositions)
            {
                _spawnPoints.Add(new SpawnPointData { ConnectionId = null, Transform = startPosition });
            }
        }
        
        private Transform ReservePosition(int connectionId)
        {
            var spawnPointDatas = _spawnPoints.Where(x => x.ConnectionId is null).ToList();
            var spawnPointData = spawnPointDatas[Random.Range(0, spawnPointDatas.Count - 1)];
            if (spawnPointData == null) return null;
            spawnPointData.ConnectionId = connectionId;
            return spawnPointData.Transform;
        }

        [Server]
        public void DeletePlayerSpawnInformation(int connectionId)
        {
            var spawnPointData =
                _spawnPoints.FirstOrDefault(x => x.ConnectionId == connectionId);

            if (spawnPointData != null)
                spawnPointData.ConnectionId = null;
        }

        [Server]
        public Player CreatePlayer(int connectionId)
        {
            var spawnTransform = ReservePosition(connectionId);
            var playerGameObject = Instantiate(NetworkManager.singleton.playerPrefab, spawnTransform.position,
                Quaternion.identity);
            var player = playerGameObject.GetComponent<Player>();
            player.Init(_gameSystem);
            return player;
        }
    }
    
    public class SpawnPointData
    {
        public Transform Transform { get; set; }
        public int? ConnectionId { get; set; }
    }
}