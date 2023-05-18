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
    public bool HasWinner => _hasWinner;
    private readonly List<Player> _players = new();
    
    public void StartGame()
    {
        RpcShowPrepareScreen();
        StartCoroutine(WaitRestartGame(_timeToPrepareRestart));
    }

    [ClientRpc]
    private void RpcShowPrepareScreen()
    {
        _prepareGameUI.ShowPrepareScreen();
    }

    private void OnPlayerScoreChanged(int score, string nickname)
    {
        if (_scoreToWin > score || _hasWinner) return;
        _hasWinner = true;
        ShowWinner(nickname);
    }
    
    private void ShowWinner(string nickname)
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
        NetworkManager.singleton.ServerChangeScene(NetworkManager.singleton.onlineScene);
    }
    
    [Server]
    public void AddPlayer(Player player)
    {
        if (_players.Contains(player))
            return;

        player.ScoreChanged += OnPlayerScoreChanged;
        _players.Add(player);
    }

    [Command(requiresAuthority = false)]
    public void CmdRemovePlayer(Player player)
    {
        RemovePlayer(player);
    }
    
    [Server]
    public void RemovePlayer(Player player)
    {
        if (_players.Contains(player) == false)
            return;

        player.ScoreChanged -= OnPlayerScoreChanged;
        _players.Remove(player);
    }
}