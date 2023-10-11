using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoundsChecker 
{
    public static int OutOfRangeDegree(List<Piece> piecesList)
    {
        int currentDegree = 0;
        int boardWidth = Board.Instance.Width - 1;

        foreach(var piece in piecesList)
        {
            piece.SetCurrentPositionValues();
            if (piece.xPos < Mathf.Min(0, currentDegree))
            {
                currentDegree = piece.xPos;
            }
            else if (piece.xPos > Mathf.Max(currentDegree, boardWidth))
            {
                currentDegree = piece.xPos;
            }
        }

        return currentDegree > boardWidth ? currentDegree - boardWidth : currentDegree;
    }
}
