using UnityEngine;
using System.Threading.Tasks;
using System;

public class Piece : MonoBehaviour
{
    #region Fields&Properties
    public Action OnInactivated;
    public int xPos => _xPos;
    public int yPos => _yPos;

    private int _xPos;
    private int _yPos;
    private Shape _parentShape;
    private Board _board;
    private PieceMoveHandler _pieceMoveHandler;
    private IFlasher _pieceFlasher;
    private SpriteRenderer _renderer;
    private Vector2 _localSpawnPos;
    #endregion

    #region Mono Methods
    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        _pieceMoveHandler.OnPiecePlaced += PiecePlaced;

    }

    private void Start()
    {
        _pieceFlasher = new PieceFlasher(_renderer);
        _pieceFlasher.SetMaterial();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameRunning)
            return;
        _pieceMoveHandler.Tick();
    }
    private void OnDisable()
    {
 
        _pieceMoveHandler.OnPiecePlaced -= PiecePlaced;
        _parentShape.OnSpawn -= SetCurrentPositionValues;
        _parentShape.OnPlacedOnGround -= PiecePlaced;
    }

    #endregion

    #region Public Methods

    public void RegisterParentShape(Shape parentShape)
    {
        _parentShape = parentShape;
        RegisterParentEvents();
    }

    public bool CheckForNextPosition(int sideToCheck)
    {
        SetCurrentPositionValues();

        if (yPos > _board.Height - 1)
            return true;

        else if (yPos == 0)
            return false;

        int xToCheck = xPos + sideToCheck;
        return CheckBelowPoint(xToCheck);
    }

    public void SetCurrentPositionValues()
    {
        _xPos = Mathf.RoundToInt(transform.position.x);
        _yPos = Mathf.RoundToInt(transform.position.y);
    }

    public async Task MatchedInARaw()
    {
        await _pieceFlasher.StartFlashing();
        PieceDown();
    }

    public void AssignNewRaw(int moveAmount) 
    {
        int newYPos =Mathf.Max(0, yPos - moveAmount);
        _pieceMoveHandler.NewRawAssigned(xPos, newYPos);
    }

    public void TriggerMovement()
    {
        _board.allBgTiles[xPos, yPos].CellStatusChanged(false);
        _pieceMoveHandler.ReadyToMove();
    }

    public bool CheckForRotatedPosition()
    {
        if (_board.allBgTiles[(int)transform.position.x, (int)transform.position.y].IsFilled)
            return false;
        return true;
    }

    public void PieceDown(bool fromShape = false)
    {
        _pieceMoveHandler.Inactivated();
        transform.localPosition = _localSpawnPos;
        if(!fromShape)
            _parentShape.PieceDestroyed();
        gameObject.SetActive(false);
    }

    #endregion

    #region Private Methods
    private void Init()
    {
        _board = Board.Instance;
        if (_board == null)
            Debug.Log("The board is null");
        _renderer = GetComponent<SpriteRenderer>();
        if (_renderer == null)
            Debug.Log("The piece could not gather its material");
        _pieceMoveHandler = new PieceMoveHandler(transform);
        _localSpawnPos = transform.localPosition;
    }

    private void RegisterParentEvents()
    {
        _parentShape.OnSpawn += SetCurrentPositionValues;
        _parentShape.OnPlacedOnGround += PiecePlaced;
    }

    private bool CheckBelowPoint(int xToCheck)
    {
        if (xToCheck < 0 || xToCheck > _board.Width-1)
            return false;

        if (_board.allBgTiles[xToCheck, yPos - 1].IsFilled)
            return false;
        else
            return true;
    }

    private void PiecePlaced() 
    {
        SetCurrentPositionValues();
        if (yPos > _board.Height - 1)
        {
            GameManager.Instance.GameEnded(false);
            return;
        }
        _board.allBgTiles[xPos, yPos].CellStatusChanged(true, this);
    }

    #endregion

}
