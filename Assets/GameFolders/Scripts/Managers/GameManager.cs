
using UnityEngine;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class GameManager : Singleton<GameManager>
{
    #region Fields&Properties
    public Action OnGameStarted;
    public Action OnGameStopped;
    public Action<bool> OnGameEnd;

    //public float GameSpeed => _gameSpeed;
    public bool IsGameRunning => _isGameRunning;
    public Material glowMaterial;

    //[SerializeField] private float _gameSpeed = 0.85f; //
    private bool _isGameRunning = false;
    #endregion

    #region MonoMethods

    #endregion

    #region PublicMethods
    public void GameStarting()
    {
        _isGameRunning = true;
        OnGameStarted?.Invoke();
    }

    public async void GameEnded(bool hasWon)
    {
        _isGameRunning = false;
        OnGameEnd?.Invoke(hasWon);
        GameStopped();
        //if (hasWon)
        //    await Awaitable.WaitForSecondsAsync(0.5f);
    }

    public void GameStopped()
    {
        OnGameStopped?.Invoke();
        Time.timeScale = 0;
    }
    #endregion


}
