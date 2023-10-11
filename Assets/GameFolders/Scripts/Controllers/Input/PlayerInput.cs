using System;

public class PlayerInput:IPlayerInput
{
    #region Fields&Properties
    public int HorizontalInput => _horizontalInput;
    public int VerticalInput => _verticalInput; 
    public bool FastDownActive => _fastDownActive;
    public event Action OnPlayerRotate;
    public event Action<int> OnPlayerHorizontalHit;

    private PlayerInputData _playerInputData;
    private int _horizontalInput, _verticalInput;
    private bool _fastDownActive;
    #endregion

    public PlayerInput()
    {
        _playerInputData = new PlayerInputData();
        _playerInputData.Enable();
        _playerInputData.Player.MoveSideways.started += OnPlayerSideMovement;
        _playerInputData.Player.Rotate.performed += OnRotatePressed;
        _playerInputData.Player.FastDown.started += (ctx) => { _fastDownActive = true; };
        _playerInputData.Player.FastDown.canceled += (ctx) => { _fastDownActive = false; };
    }

    private void OnRotatePressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayerRotate?.Invoke();
    }

    private void OnPlayerSideMovement(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _horizontalInput = (int)obj.ReadValue<float>();
        OnPlayerHorizontalHit(_horizontalInput);
    }
}
