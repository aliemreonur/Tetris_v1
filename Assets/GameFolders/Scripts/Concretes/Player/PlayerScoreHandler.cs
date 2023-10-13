
public class PlayerScoreHandler : IPlayerScorer
{
    public int TotalScore { get; private set; }
    public byte RawsScore { get; private set; }

    private byte _currentRawGoal;
    private PlayerDataHandler _playerDataHandler;
    private int _initialScore;

    public PlayerScoreHandler(PlayerDataHandler playerDataHandler, PlayerData playerData)
    {
        _playerDataHandler = playerDataHandler;
        _initialScore = playerData.PlayerScore;
        TotalScore = _initialScore;
        RawsScore = 0;
        GameManager.Instance.OnGameStopped += ResetScores;
    }

    public void RawDestroyed(byte rawAmount)
    {
        RawsScore += rawAmount;
        TotalScore += rawAmount*rawAmount * 300 ; 
        PlayerScoreUpdated();

        if (RawsScore>=_currentRawGoal)
        {
            _playerDataHandler.LevelUp();
            RawsScore = 0;
            PlayerScoreUpdated();
        }
    }

    public void SetRawSuccessAmount(byte newSuccessAmount)
    {
        _currentRawGoal = newSuccessAmount;
        PlayerScoreUpdated();
    }

    public void DeregisterEvents()
    {
        GameManager.Instance.OnGameStopped -= ResetScores;
    }

    private void PlayerScoreUpdated()
    {
        UIManager.Instance.SetRawsUI(RawsScore, _currentRawGoal, TotalScore, _playerDataHandler.PlayerLevel);
    }

    private void ResetScores()
    {
        RawsScore = 0;
        TotalScore = _initialScore;
        PlayerScoreUpdated();
    }

}
