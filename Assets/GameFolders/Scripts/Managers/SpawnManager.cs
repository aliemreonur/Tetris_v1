using UnityEngine;
using System;

public class SpawnManager : Singleton<SpawnManager>
{
    #region Fields&Properties
    public Action<int, Shape> OnShapeSpawned;
    public byte NextShapeID => _nextShapeID;
    private byte _nextShapeID;
    private PlayerController _playerController;
    private Board _boardManager;
    private PoolManager _poolManager;
    #endregion

    #region MonoMethods
    private void Start()
    {
        _poolManager = PoolManager.Instance;
        _boardManager = Board.Instance;
        _playerController = PlayerController.Instance;
        GameManager.Instance.OnGameStarted += Spawn;
        AssignNextShape();
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStarted -= Spawn;
    }
    #endregion

    #region Public Methods
    public void Spawn()
    {
        if (!GameManager.Instance.IsGameRunning)
            return;

        Vector2 spawnPos = AssignSpawnPoint();
        Shape shapeToSpawn = _poolManager.RequestShape(_nextShapeID, spawnPos);
        OnShapeSpawned?.Invoke(_nextShapeID, shapeToSpawn);
        _playerController.AssignNewShape(shapeToSpawn);
        AssignNextShape();
    }
    #endregion

    #region Private Methods
    private void AssignNextShape()
    {
        _nextShapeID = (byte)AssignRandomShape();
        UIManager.Instance.SetNextShapeImage(_nextShapeID);
    }

    private int AssignRandomShape()
    {
        return UnityEngine.Random.Range(0, 7);
    }

    private Vector2 AssignSpawnPoint()
    {
        //int randomX = UnityEngine.Random.Range(0, Mathf.RoundToInt(_boardManager.Width/2));
        int xValue = Mathf.RoundToInt(_boardManager.Width / 2);
        int spawnY = _boardManager.Height + 1;

        Vector2Int spawnPos = new Vector2Int(xValue, spawnY);
        return spawnPos;
    }
    #endregion

}
