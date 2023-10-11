using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;

public class CountDowner 
{
    private byte _scaleMultiplier = 3;

    public async Task StartCountDown(TextMeshProUGUI countDownText)
    {
        byte _startFrom = 3;
        countDownText.SetText("");
        countDownText.rectTransform.localScale = Vector3.zero;
        countDownText.gameObject.SetActive(true);
        countDownText.rectTransform.DOScale(_scaleMultiplier, 0.5f);

        do
        {
            countDownText.SetText(_startFrom.ToString());
            countDownText.rectTransform.DOScale(_scaleMultiplier, 0.75f).OnComplete(() => countDownText.rectTransform.DOScale(0, 0.25f));
            await Awaitable.WaitForSecondsAsync(1f);
            _startFrom--;
            if (_startFrom < 0)
                _startFrom = 0;
        }

        while (_startFrom > 0);

        //countDownText.gameObject.SetActive(false);
    }
}
