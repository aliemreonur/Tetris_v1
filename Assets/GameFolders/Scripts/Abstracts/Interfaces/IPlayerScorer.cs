

public interface IPlayerScorer
{
    int TotalScore { get; }
    byte RawsScore { get; }
    void RawDestroyed(byte rawAmount);
    void SetRawSuccessAmount(byte newSuccessAmount);
}
