using System.Collections.Generic;
using Mirror;
using PlayerComponents;

namespace Game
{
    public abstract class GameSystem : NetworkBehaviour
    {
        protected readonly List<Player> players = new();
        public bool HasWinner { get; protected set; }

        public abstract void StartGame();
        public abstract void EndGame(string nickname);
        protected abstract void OnPlayerAdd(Player player);
        protected abstract void OnPlayerRemove(Player player);
        
        [Server]
        public void AddPlayer(Player player)
        {
            if (players.Contains(player))
                return;
            OnPlayerAdd(player);
            players.Add(player);
        }

        [Command(requiresAuthority = true)]
        public void CmdRemovePlayer(Player player)
        {
            RemovePlayer(player);
        }

        [Server]
        public void RemovePlayer(Player player)
        {
            if (players.Contains(player) == false)
                return;
            OnPlayerRemove(player);
            players.Remove(player);
        }
    }
}