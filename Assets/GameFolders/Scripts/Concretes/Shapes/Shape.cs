using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Shape : MonoBehaviour
{
    #region Fields&Properties
    public List<Piece> shapePieces => _pieces;
    public Action OnPlacedOnGround;
    public Action OnSpawn;
    public Action OnInactivated;

    [SerializeField] private bool _canRotate = true; //this is shorutcut fix for square rotation control
    private List<Piece> _pieces;
    private bool _isMoving;
    private IShapeRotator _shapeRotator;
    private IMovementChecker _movementChecker;
    #endregion

    #region MonoMethods
    private void Awake()
    {
        _shapeRotator = new ShapeRotator(this);
        _movementChecker = new MovementChecker(this);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStopped += DisableObject;
        Initialize();
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStopped -= DisableObject;
    }

    #endregion

    #region Public Methods
    public void Initialize( )
    {
        GatherPieces();
        _isMoving = true;
        OnSpawn?.Invoke();
        _shapeRotator.SpawnRotation();
    }

    public void Move(int sideMovement)
    {
        _movementChecker.Move(sideMovement);
    }

    public void CorrectiveSideMovement()
    {
        int boundsNotification = BoundsChecker.OutOfRangeDegree(_pieces);
        if (boundsNotification == 0)
            return;

        transform.position = new Vector2(transform.position.x - boundsNotification, transform.position.y);

        foreach (var piece in _pieces)
            piece.SetCurrentPositionValues();
    }

    public void PieceDestroyed()
    {
        bool allDestroyed = true;

        foreach(var piece in _pieces)
        {
            if (piece.gameObject.activeSelf)
            {
                allDestroyed = false;
            }
        }

        if(allDestroyed)
        {
            gameObject.SetActive(false);
        }

    }

    public void RotateShape()
    {
       if (!_canRotate)
            return;
       if (!_isMoving || !GameManager.Instance.IsGameRunning)
            return;
        _shapeRotator.RotateShape();
    }

    public void ShapeOnTheGround()
    {
        _isMoving = false;
        OnPlacedOnGround?.Invoke();
    }

    #endregion

    #region Private Methods
    private void GatherPieces()
    {
        if (transform.childCount == 0)
            return;
 
        _pieces = GetComponentsInChildren<Piece>(true).ToList();
        if (_pieces.Count == 0)
            Debug.Log("The shape could not gather its pieces");

        foreach (var piece in _pieces)
        {
            piece.RegisterParentShape(this);
            if(!piece.gameObject.activeInHierarchy)
                piece.gameObject.SetActive(true); //this is for if its a recycled object
        }
    }

    private void DisableObject()
    {
        OnInactivated?.Invoke();
        foreach (var piece in _pieces)
            piece.PieceDown(true);
        gameObject.SetActive(false);
    }
    #endregion
}
