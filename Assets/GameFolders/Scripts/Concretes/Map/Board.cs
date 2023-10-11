using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class Board : Singleton<Board>
{
    #region Fields&Properties
    public Action<byte,byte> OnMapSet;
    public Action OnBoardLoaded;
    public BgTile[,] allBgTiles => _allBgTiles;

    public byte Width { get; private set; }
    public byte Height { get; private set; }

    private BgTile[,] _allBgTiles;
    private MatchHandler _matchHandler;
    private PlayerDataHandler _playerDataHandler;
    private List<BgTile> _activeTiles = new();
    private BackgroundLoader _backgroundLoader;
    private BackgroundAdjuster _bgAdjuster;
    #endregion

    #region MonoFunctions
    private void Start()
    {
        _playerDataHandler = new PlayerDataHandler();
        UIManager.Instance.OnPlayerStarted += CleanBoard;
    }

    private void OnDisable()
    {
        UIManager.Instance.OnPlayerStarted -= CleanBoard;
    }
    #endregion

    #region Public Methods

    public void CheckForMatches()
    {
        _matchHandler.CheckForMatches();
    }

    public async Task InitializeBoard(byte width, byte height)
    {
        if (_backgroundLoader == null)
        {
            _backgroundLoader = new BackgroundLoader();
            _backgroundLoader.OnPrefabLoaded += InstantiateTheBoard;
            await _backgroundLoader.LoadBackgroundTile();
        }
        CleanBoard();
        SetBoardDimensions(width, height);
        CreateBoard();
        AssignBgTiles();
        OnBoardLoaded?.Invoke();
    }

    public void ThemeChanged(Sprite newSprite)
    {
        _bgAdjuster.ChangeSprite(newSprite);
    }
    #endregion

    #region Private Methods
    private void CreateBoard()
    {
        _allBgTiles = new BgTile[Width, Height];
        if(_matchHandler == null)
            _matchHandler = new MatchHandler(this, _playerDataHandler.PlayerScoreHandler);
    }

    private void InstantiateTheBoard(BackgroundAdjuster bgPrefab)
    {
        _bgAdjuster = Instantiate(bgPrefab, transform);
    }

    private void AssignBgTiles()
    {
        for (byte y=0; y<Height; y++)
        {
            for(byte x=0; x<Width; x++)
            {
                BgTile bgTile = new BgTile(x, y);
                SetTileProperties(y, x, bgTile);
            }
        }
    }

    private void SetTileProperties(byte y, byte x, BgTile bgTile)
    {
        _allBgTiles[x, y] = bgTile;
        _activeTiles.Add(bgTile);
    }

    private void SetBoardDimensions(byte width, byte height)
    {
        Width = width;
        Height = height;
        OnMapSet?.Invoke(Width, Height);
    }

    private void CleanBoard()
    {
        if (_activeTiles.Count == 0)
            return;
        foreach(var tile in _activeTiles)
        {
            tile.ResetTile();
        }
        _activeTiles.Clear();
    }
    #endregion
}
