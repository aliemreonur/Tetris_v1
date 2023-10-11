using UnityEngine;
using System;

[System.Serializable]
public class PieceMoveHandler : ITicker
{
    #region Fields&Properties
    public Vector2 TargetPos => _targetPos;
    public Action OnPiecePlaced;
    private Transform _pieceTransform;
    private Vector2 _targetPos;
    private bool _isMoving = false;
    #endregion

    #region Public Methods&Const
    public PieceMoveHandler(Transform pieceTransform)
    {
        _pieceTransform = pieceTransform;
        _targetPos = Vector2.negativeInfinity;
    }

    public void Tick()
    {
        if (!_isMoving)
            return;

        MoveDown();
    }

    public void MoveDown()
    {
        if (_targetPos == Vector2.negativeInfinity)
            return;

        Vector2 newPos = _targetPos;
        _pieceTransform.position = Vector2.Lerp(_pieceTransform.position, _targetPos, Time.deltaTime * 10f);
        if (_pieceTransform.position.y - _targetPos.y < 0.2f)
        {
            _pieceTransform.position = _targetPos;
            OnPiecePlaced?.Invoke();
            _isMoving = false;
        }
    }

    public void NewRawAssigned(int xPos, int newY)
    {
        _targetPos = new Vector2(xPos, newY);
    }

    public void ReadyToMove()
    {
        _isMoving = true;
    }

    public void Inactivated()
    {
        _isMoving = false;
        _targetPos = Vector2.negativeInfinity;
    }
    #endregion
}
