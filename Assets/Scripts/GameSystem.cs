using System.Collections;
using System.Collections.Generic;
using Mirror;
using PlayerComponents;
using UI;
using UnityEngine;

public class GameSystem : NetworkBehaviour
{
    [SerializeField] private int _scoreToWin = 3;
    [SerializeField] private float _timeToShowWinner = 2f;
    [SerializeField] private float _timeToPrepareRestart = 2f;
    [SerializeField] private WinnerUI _winnerUI;
    [SerializeField] private PrepareGameUI _prepareGameUI;
    private bool _hasWinner;
    private readonly List<Player> _players = new();

    public void AddPlayer(Player player)
    {
        if (_players.Contains(player))
            return;

        player.ScoreChanged += OnPlayerScoreChanged;
        _players.Add(player);
    }

    [ClientRpc]
    public void StartGame()
    {
        _prepareGameUI.ShowPrepareScreen();
        StartCoroutine(WaitRestartGame(_timeToPrepareRestart));
    }

    private void OnPlayerScoreChanged(int score, string nickname)
    {
        if (_scoreToWin > score || _hasWinner) return;
        _hasWinner = true;
        RpcShowWinner(nickname);
    }

    [ClientRpc]
    private void RpcShowWinner(string nickname)
    {
        StartCoroutine(ShowWinner(nickname));
    }

    private IEnumerator ShowWinner(string nickname)
    {
        _winnerUI.ShowWinner(nickname);
        yield return WaitRestartGame(_timeToShowWinner);
    }

    private IEnumerator WaitRestartGame(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        RestartGame();
    }

    private void RestartGame()
    {
        if (isServer)
            NetworkManager.singleton.ServerChangeScene(NetworkManager.singleton.onlineScene);
    }

    public void RemovePlayer(Player player)
    {
        if (_players.Contains(player) == false)
            return;

        player.ScoreChanged -= OnPlayerScoreChanged;
        _players.Remove(player);
    }
}