using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDataHandler : IPlayerDataHandler
{
    public Action OnNextLevelAchieved;
    public byte PlayerLevel { get; private set; }
    //public int PlayerScore => _playerScoreHandler.TotalScore;
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

    private void LoadPlayerData() //this does not needs to be public
    {
        if (_playerData != null) //we have already gathered the data
            return;

        _playerData = SaveLoadSystem.LoadData();
        //PlayerLevel = _playerData.LevelID;
        PlayerLevel = 1; //tbd

        _playerScoreHandler = new PlayerScoreHandler(this, _playerData);
        _playerData.SetPlayerData(PlayerLevel, 0); //tbd
        

        //if not test 
        LevelManager.Instance.AssignPlayerData(this);
    }

}
