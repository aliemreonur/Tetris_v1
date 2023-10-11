
using UnityEngine;
using System;

public interface IPlayerController 
{
    int PlayerSidewaysMovement { get; }
    bool FastDownOn { get; }
    event Action<Shape> OnShapeChanged;
    void ResetHorizontalInput();
}
