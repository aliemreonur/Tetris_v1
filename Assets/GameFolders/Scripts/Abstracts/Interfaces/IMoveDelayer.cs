
public interface IMoveDelayer : ITicker
{
    bool CanMove { get; }
    void ChangeSpeed(float newValue);
}
