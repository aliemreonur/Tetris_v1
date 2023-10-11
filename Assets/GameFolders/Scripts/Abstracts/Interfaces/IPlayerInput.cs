using System;
using UnityEngine;

public interface IPlayerInput 
{
    int HorizontalInput { get; }
    int VerticalInput { get; }
    bool FastDownActive { get; }
    event Action OnPlayerRotate;
    event Action<int> OnPlayerHorizontalHit;
    //event Action<Vector2> OnPlayerMovement;
}
