using System.Collections;
using Mirror;
using UI;
using UnityEngine;

namespace Game
{
    public abstract class BaseGameSystem : GameSystem
    {
        [SerializeField] protected float _timeToShowWinner = 2f;
        [SerializeField] protected float _timeToPrepareRestart = 2f;
        [SerializeField] protected WinnerUI _winnerUI;
        [SerializeField] protected PrepareGameUI _prepareGameUI;
        
        public override void StartGame()
        {
            RpcShowPrepareScreen();
            StartCoroutine(WaitRestartGame(_timeToPrepareRestart));
        }

        [ClientRpc]
        private void RpcShowPrepareScreen()
        {
            _prepareGameUI.ShowPrepareScreen();
        }

        public override void EndGame(string nickname)
        {
            RpcShowWinner(nickname);
            StartCoroutine(WaitRestartGame(_timeToShowWinner));
        }

        [ClientRpc]
        private void RpcShowWinner(string nickname)
        {
            _winnerUI.ShowWinner(nickname);
        }

        private IEnumerator WaitRestartGame(float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);
            RestartGame();
        }

        [Server]
        private void RestartGame()
        {
            HasWinner = false;
            NetworkManager.singleton.ServerChangeScene(NetworkManager.singleton.onlineScene);
        }
    }
}