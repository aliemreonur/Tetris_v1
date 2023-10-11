using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// These can be assigned to a predefined sprite and positioned accordingly,
/// For now, these are attached as a game object in the game, causing unnecessary drawcalls.
/// </summary>
public class BgTile 
{
    #region Fields&Properties
    public Piece currentPiece => _currentPiece;
    public byte xPos, yPos;
    public bool IsFilled => _isFilled;
    private Piece _currentPiece;
    private bool _isFilled;
    #endregion

    public BgTile(byte x, byte y)
    {
        xPos = x;
        yPos = y;
        _currentPiece = null;
        _isFilled = false;
    }

    #region Public Methods
    public void SetValues(byte x, byte y)
    {
        xPos = x;
        yPos = y;
    }

    public void CellStatusChanged(bool isFilled, Piece pieceToFill = null)
    {
        _isFilled = isFilled;
        if (isFilled)
            _currentPiece = pieceToFill;
    }

    public async Task HadAMatch()
    {
        if (_currentPiece != null)
        {
            await _currentPiece.MatchedInARaw();
            _isFilled = false;
        }
    }

    public void ResetTile()
    {
        if (_currentPiece != null)
            _currentPiece.PieceDown();
        _isFilled = false;
        _currentPiece = null;
        xPos = 0;
        yPos = 0;
    }
    #endregion
}
