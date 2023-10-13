 
using UnityEngine;

public class MovementChecker : IMovementChecker
{
    #region Fields&Properties
    public int SideMovement;
    private Shape _shape;
    private Transform _shapeTransform;
    private int _sideMovement;
    #endregion

    #region Public Methods&Const
    public MovementChecker(Shape shape)
    {
        _shape = shape;
        _shapeTransform = _shape.transform;
    }

    public void Move(int sideValue)
    {
        _sideMovement = sideValue;
        if (!CheckForMovement(_sideMovement))
            return;

        Vector2 posToMove = new Vector2(_sideMovement < 0 ? _shapeTransform.position.x - 1 : _shapeTransform.position.x + _sideMovement,
            _shapeTransform.position.y - 1);
        _shapeTransform.position = posToMove;
    }

    public bool CheckForMovement(int sideMovement)
    {
        foreach (var piece in _shape.shapePieces)
        {
            if (!piece.CheckForNextPosition(sideMovement))
            {
                if (sideMovement != 0)
                {
                    _sideMovement = 0;
                    return CheckForMovement(0);
                }
                _shape.ShapeOnTheGround();
                SpawnManager.Instance.Spawn();
                Board.Instance.CheckForMatches();

                return false;
            }
        }
        return true;
    }
    #endregion
}
