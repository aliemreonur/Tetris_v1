using UnityEngine;

public class MoveDelayer : IMoveDelayer
{
    #region Fields&Properties
    public bool CanMove => _canMove;

    private bool _canMove;
    private float _delayTime;
    private float _lastMovementTime = 0;
    private float _normalDelayTime;
    private Shape _currentShape;
    private IPlayerController _playerController;
    #endregion

    #region Public Methods&Const
    public MoveDelayer(IPlayerController playerController)
    {
        _normalDelayTime = 0.15f; //default
        _delayTime = _normalDelayTime;
        _playerController = playerController;
        _playerController.OnShapeChanged += OnShapeChanged;
    }

    public void Tick()
    {
        if (!_canMove || _currentShape == null || !GameManager.Instance.IsGameRunning)
            return;

        SetSpeed();

       if(Time.time>_lastMovementTime + _delayTime)
       {
            _currentShape.Move(_playerController.PlayerSidewaysMovement);
            _lastMovementTime = Time.time;
            _playerController.ResetHorizontalInput();
       }
    }

    public void ChangeSpeed(float newValue)
    {
        _normalDelayTime = 1-newValue;
    }
    #endregion

    #region Private Methods
    private void SetSpeed()
    {
        _delayTime = _playerController.FastDownOn ? _normalDelayTime / 3 : _normalDelayTime;
    }

    private void OnShapeChanged(Shape newShape)
    {
        if(_currentShape != null)
            _currentShape.OnPlacedOnGround -= StopMovement;
        _currentShape = newShape;
        _currentShape.OnPlacedOnGround += StopMovement;
        _canMove = true;
    }

    private void StopMovement()
    {
        _canMove = false;
    }
    #endregion
}
