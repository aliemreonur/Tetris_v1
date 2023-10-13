using System.Collections.Generic;
using System.Threading.Tasks;

public class MatchHandler 
{
    #region Fields&Properties
    private List<int> _filledRaws;
    private Board _board;
    private List<Piece> _piecesToMove;
    private List<BgTile> _matchedTiles;
    private IPlayerScorer _playerScoreHandler;
    private byte _numberOfRawsMatched;
    #endregion

    #region Public Methods & Const
    public MatchHandler(Board board, IPlayerScorer playerScoreHandler)
    {
        _board = board;
        _filledRaws = new List<int>();
        _piecesToMove = new();
        _matchedTiles = new();
        _playerScoreHandler = playerScoreHandler;
    }

    public async void CheckForMatches()
    {
        ClearLists();

        for (int i = 0; i < _board.Height - 1; i++)
        {
            if (!CheckRawForMatch(i))
                continue;
        }
         _numberOfRawsMatched = (byte)_filledRaws.Count;
        if (_numberOfRawsMatched > 0)
        {
            foreach (var filledRaw in _filledRaws.ToArray())
                      AssignRawTiles(filledRaw);

            await PiecesMatchedGoingOff();

            if (!GameManager.Instance.IsGameRunning)
                return;

            AssignPiecesToMove();
            MoveDownPieces();
            _playerScoreHandler.RawDestroyed(_numberOfRawsMatched);
        }
    }
    #endregion

    #region Private Methods

    private void ClearLists()
    {
        _numberOfRawsMatched = 0;
        _filledRaws.Clear();
        _piecesToMove.Clear();
        _matchedTiles.Clear();
    }

    private bool CheckRawForMatch(int rawToCheck)
    {
        for(int j=0; j<_board.Width; j++)
        {
            if (!_board.allBgTiles[j, rawToCheck].IsFilled)
                return false;
        }
        _filledRaws.Add(rawToCheck);
        return true;
    }

    private void AssignRawTiles(int rawId)
    {
        for(int i=0; i<_board.Width; i++)
        {
            _board.allBgTiles[i, rawId].CellStatusChanged(false); //this is also getting null from the piece
            _matchedTiles.Add(_board.allBgTiles[i,rawId]);
        }
    }

    private async Task PiecesMatchedGoingOff()
    {
        var tasks = new Task[_matchedTiles.Count];

        for(int i=0; i<_matchedTiles.Count; i++)
        {
            tasks[i] = _matchedTiles[i].HadAMatch();
        }

        await Task.WhenAll(tasks);
  
    }

    private void AssignPiecesToMove()
    {
        int currentCount = 0;

        for(int i=0; i<_board.Height; i++)
        {
            if (_filledRaws.Contains(i))
                currentCount++;

            else
            {
                for (int j = 0; j < _board.Width; j++)
                {
                    Piece pieceToGoDown = _board.allBgTiles[j, i].currentPiece;
                    if (pieceToGoDown == null)
                        continue;

                    _piecesToMove.Add(pieceToGoDown);
                    pieceToGoDown.AssignNewRaw(currentCount);          
                }
            }
        }
    }

    private void MoveDownPieces()
    {
        foreach(var piece in _piecesToMove)
        {
            piece.TriggerMovement();
        }
    }
    #endregion
}
