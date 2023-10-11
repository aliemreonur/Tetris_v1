using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// restart not working
/// 
/// </summary>
public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Image _themeImg;
    [SerializeField] private Slider _gameSpeedSlider;
    [SerializeField] private Slider _gameMusicSlider;
    [SerializeField] private TextMeshProUGUI _speedText;
    //gather the current theme image from player prefs?

    private int _currentThemeNumber=1;
    private float _speed;
    private int _volume;

    private ThemeChanger _themeChanger;
    private const string VOLUME = "Volume";
    private const string GAME_SPEED = "Game Speed";


    private void Start()
    {
        SetMenuValues();

        Board.Instance.OnBoardLoaded += InitializeThemeChanger;
    }

    private void SetMenuValues()
    {
        _volume = PlayerPrefs.HasKey(VOLUME) ? PlayerPrefs.GetInt(VOLUME) : 80;
        _speed = PlayerPrefs.HasKey(GAME_SPEED) ? PlayerPrefs.GetFloat(GAME_SPEED) : 0.85f;
        _gameSpeedSlider.value = _speed;
        SetSpeedText();

    }

    private void SetSpeedText()
    {
        int roundedSpeed = Mathf.RoundToInt(_speed * 100);
        _speedText.SetText(roundedSpeed.ToString());
    }

    #region Public Methods

    public void SetNewGameSpeed()
    {
        //update game manager speed
        _speed = _gameSpeedSlider.value;
        _speed = Mathf.Clamp(_speed, 0.15f, 0.99f);
        SetSpeedText();
        PlayerController.Instance.SetPlayerSpeed(_speed);
        PlayerPrefs.SetFloat(GAME_SPEED, _speed);


        //_speed = Mathf.RoundToInt(_gameSpeedSlider.value*100);
        //_speed = Mathf.Clamp(_speed, 10, 100);
        //Update the settings
        //Throw an action
    }

    public void SetVolume()
    {
        //update game manager speed
        _volume = Mathf.RoundToInt(_gameMusicSlider.value *100);
        PlayerPrefs.SetInt(VOLUME, _volume);
        //Update the settings
        //Throw an action
    }


    public void ChangeTheme(int increment)
    {
        _currentThemeNumber += increment;
        _currentThemeNumber = _currentThemeNumber == 5 ? _currentThemeNumber = 1 : _currentThemeNumber <= 0 ? _currentThemeNumber = 4 : _currentThemeNumber;
        LoadTheTheme();
    }

    public void BackToGame()
    {
        //turn the menu off
        //time.timescale 1
        //Resumne the game

        PlayerPrefs.Save();
    }
    #endregion

    #region Private Methods
    private void InitializeThemeChanger()
    {
        _themeChanger = new ThemeChanger(_themeImg);
        _currentThemeNumber = _themeChanger.SpriteIndex;
        Board.Instance.OnBoardLoaded -= InitializeThemeChanger;
    }

    private void LoadTheTheme()
    {
        _themeChanger.LoadSpriteWithIndex(_currentThemeNumber);
    }
    #endregion
}
