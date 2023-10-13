using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
using System;

public class LevelManager : Singleton<LevelManager>
{
    #region Fields&Properties
    public Action OnPlayerPassedLevel;
    public Action OnNewLevelLoaded;

    public bool TestLevel;
    public byte LevelHeight => _levelHeight;
    public byte LevelWidth => _levelWidth;

    private AsyncOperationHandle<ScriptableObject> _loadLevelHandle; //load the map on a seperate loader 
    private byte _levelHeight, _levelWidth;
    private PlayerDataHandler _playerDataHandler;
    private byte _currentPlayerLevel = 1;

    #endregion

    #region Mono Methods

    private void Start()
    {
        GameManager.Instance.OnGameStopped += LoadNewLevel;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStopped -= LoadNewLevel;
        _playerDataHandler.OnNextLevelAchieved -= PlayerLevelUpdated;
    }
    #endregion

    #region Public Methods
    public async void ReloadLevel()
    {
        await LoadTiles();
    }

    public void AssignPlayerData(PlayerDataHandler playerDataHandler)
    {
        _playerDataHandler = playerDataHandler;
        _currentPlayerLevel = playerDataHandler.PlayerLevel;
        playerDataHandler.OnNextLevelAchieved += PlayerLevelUpdated;
        LoadNewLevel();
    }

    public void LoadNewLevel()
    {
        _loadLevelHandle = Addressables.LoadAssetAsync<ScriptableObject>($"Level{_currentPlayerLevel}SO");
        _loadLevelHandle.Completed += OnLevelLoaded;
    }
    #endregion

    #region Private Methods

    private async void OnLevelLoaded(AsyncOperationHandle<ScriptableObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            LevelSO loadedLevel = (LevelSO)obj.Result;
            SetLevelProperties(loadedLevel);
            await LoadTiles();
            OnNewLevelLoaded?.Invoke();
            //PlayerScored(0, 0);
        }
        else if (obj.Status == AsyncOperationStatus.Failed)
        {
            Debug.LogError("Failed to load level, Loading Level 1");
            _currentPlayerLevel = 1;
            LoadNewLevel();
        }
    }

    private void SetLevelProperties(LevelSO loadedLevel)
    {
        _levelHeight = loadedLevel.levelHeight;
        _levelWidth = loadedLevel.levelWidth;
        _playerDataHandler.PlayerScoreHandler.SetRawSuccessAmount(loadedLevel.minimumRawToSuccess);
    }

    private void PlayerLevelUpdated()
    {
        _currentPlayerLevel = _playerDataHandler.PlayerLevel;
        GameManager.Instance.GameEnded(true);
        OnPlayerPassedLevel?.Invoke();
        LoadNewLevel();
    }

    private async Task LoadTiles()
    {
        await Board.Instance.InitializeBoard(_levelWidth, _levelHeight);
    }
    #endregion
}
