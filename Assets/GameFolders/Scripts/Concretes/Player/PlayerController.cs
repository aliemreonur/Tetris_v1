using System;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>, IPlayerController
{
    #region Fields&Properties
    public event Action<Shape> OnShapeChanged;

    public int PlayerSidewaysMovement => _playerSideWaysMovement;
    public bool FastDownOn => _playerInput.FastDownActive;
    private Shape _currentShape;
    
    private IPlayerInput _playerInput;
    private IMoveDelayer _moveDelayer;
    private int _playerSideWaysMovement;
    #endregion

    #region MonoMethods
    protected override void Awake()
    {
        base.Awake();
        _playerInput = new PlayerInput();
        _playerInput.OnPlayerRotate += ReturnShape;
        _playerInput.OnPlayerHorizontalHit += SetHorizontalMovementValue;
        _moveDelayer = new MoveDelayer(this);
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameRunning)
            return;

        _moveDelayer.Tick();
    }
    #endregion

    #region Public Methods
    public void ResetHorizontalInput()
    {
        //does not support hold!
        _playerSideWaysMovement = 0;
    }

    public void SetPlayerSpeed(float value)
    {
        _moveDelayer.ChangeSpeed(value);
    }

    public void AssignNewShape(Shape shapeToAssign)
    {
        OnShapeChanged?.Invoke(shapeToAssign);
        _currentShape = shapeToAssign;
    }
    #endregion

    #region Private Methods
    private void ReturnShape()
    {
        if (_currentShape == null || !GameManager.Instance.IsGameRunning)
            return;
        _currentShape.RotateShape();
    }

    //private float GetLevelDelayTime()
    //{
    //    //this will be updated as it will be pulled the delay from the scriptable object delay
    //    return 1- GameManager.Instance.GameSpeed;
    //}

    private void SetHorizontalMovementValue(int sideMovementValue)
    {
        _playerSideWaysMovement = sideMovementValue;
    }
    #endregion
}
