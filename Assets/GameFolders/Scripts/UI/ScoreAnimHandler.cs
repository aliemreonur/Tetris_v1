using TMPro;
using UnityEngine;
using System.Threading.Tasks;

public class ScoreAnimHandler 
{
    private TextMeshProUGUI _scoreText;
    private float _increaseSpeed = 0.02f;
    private int _currentScore = 0000000;
    private string NumberFormat = "N0";

    public ScoreAnimHandler(TextMeshProUGUI scoreText)
    {
        _scoreText = scoreText;
    }

    public async void PlayerScored(int newScore)
    {
        await ScoreUpdated(newScore);
    }

    private async Task ScoreUpdated(int newScore)
    {
        while(_currentScore<newScore)
        {
            await Awaitable.WaitForSecondsAsync(_increaseSpeed);
            _currentScore+=10;
            if(_currentScore >= newScore)
            {
                _currentScore = newScore;
            }
            _scoreText.SetText(_currentScore.ToString(NumberFormat));
            // await Awaitable.EndOfFrameAsync();
        }
    }


}