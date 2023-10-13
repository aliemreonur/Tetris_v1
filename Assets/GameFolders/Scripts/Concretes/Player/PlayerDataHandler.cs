using UnityEngine;
using System;

public class PlayerDataHandler : IPlayerDataHandler
{
    public Action OnNextLevelAchieved;
    public byte PlayerLevel { get; private set; }
    public int PlayerRaws => _playerScoreHandler.RawsScore;
    public IPlayerScorer PlayerScoreHandler => _playerScoreHandler;

    private IPlayerScorer _playerScoreHandler;
    private PlayerData _playerData;

    public PlayerDataHandler()
    {
        LoadPlayerData();
    }

    public void LevelUp()
    {
        PlayerLevel++;
        PlayerLevel = (byte)Mathf.CeilToInt(PlayerLevel % 6);
        _playerData.SetPlayerData(PlayerLevel, _playerScoreHandler.TotalScore);
        OnNextLevelAchieved?.Invoke();
    }

    public void DeregisterEvents()
    {
        _playerScoreHandler.DeregisterEvents();
    }

    private void LoadPlayerData() 
    {
        if (_playerData != null) 
            return;

        _playerData = SaveLoadSystem.LoadData();
        PlayerLevel = _playerData.LevelID;
        PlayerLevel = LevelManager.Instance.TestLevel ? (byte)1 : _playerData.LevelID;
        _playerScoreHandler = new PlayerScoreHandler(this, _playerData);
        if(LevelManager.Instance.TestLevel)
            _playerData.SetPlayerData(PlayerLevel, 0);

        LevelManager.Instance.AssignPlayerData(this);
    }

}
