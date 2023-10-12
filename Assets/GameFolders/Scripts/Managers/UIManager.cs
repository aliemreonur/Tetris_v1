using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using DG.Tweening;

[RequireComponent(typeof(SettingsUI))]
public class UIManager : Singleton<UIManager>
{
    #region Fields&Properties
    [Header("Panels")]
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _settingsPanel;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _headerText;
    [SerializeField] private TextMeshProUGUI _rawsText;
    [SerializeField] private TextMeshProUGUI _startButtonText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    [Header("Buttons")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _pauseButton;

    [Header("Others")]
    [SerializeField] private Sprite[] _allShapes;
    [SerializeField] private Image _nextShapeImg;

    //public Action<int> OnPlayerScored;
    public Action OnPlayerStarted;

    private const string SUCCESS_TEXT = "SUCCESS";
    private const string FAIL_TEXT = "FAIL";
    private const string RESTART_TEXT = "RESTART";
    private const string NEXT_LEVEL_TEXT = "NEXT LEVEL";
    private const string LEVEL = "Level";

    private ScoreAnimHandler _scoreAnimHandler;
    private CountDowner _countDowner;
    private SettingsUI _settingsUI;
    #endregion

    #region MonoMethods
    protected override void Awake()
    {
        base.Awake();
        Init();
    }


    private void Start()
    {
        RegisterEvents();
    }

    private void OnDisable()
    {
        DeregisterEvents();
    }
    #endregion

    #region Public Methods
    public void SetNextShapeImage(byte IdToSet)
    {
        if (IdToSet > _allShapes.Length)
        {
            _nextShapeImg.gameObject.SetActive(false); //simply turn it off
            return;
        }
        _nextShapeImg.sprite = _allShapes[IdToSet];
    }

    public async void OnStartButton()
    {
        OnPlayerStarted?.Invoke();
        _startButton.gameObject.SetActive(false);
        Time.timeScale = 1;
        SetPanelStatus(_gamePanel, true); //dotween from the rightside?
        await _countDowner.StartCountDown(_headerText);
        _startPanel.SetActive(false);
        GameManager.Instance.GameStarting();
    }

    public void RestartLevel()
    {
        //Needs Fix
        GameManager.Instance.GameStopped();
        OnStartButton();
        _pausePanel.SetActive(false);
    }

    public void SetRawsUI(byte currentRawsDown, byte targetScore, int playerScore, byte currentLevel)
    {
        _rawsText.text = currentRawsDown + "/" + targetScore;
        _scoreText.text = playerScore.ToString();
        _scoreAnimHandler.PlayerScored(playerScore);
        _levelText.text = LEVEL + " " + currentLevel.ToString();
    }

    public void OnPauseButton(bool isPauseOn) //also to be used for the back button
    {
        _pausePanel.SetActive(isPauseOn);
        _pauseButton.gameObject.SetActive(!isPauseOn);
        Time.timeScale = isPauseOn ? 0 :1;        
    }

    public void SettingsMenuActivated(bool isSettingsOn)
    {
        _pausePanel.SetActive(!isSettingsOn);
        _settingsPanel.SetActive(isSettingsOn);
    }


    #endregion

    #region Private Methods
    private void Init()
    {
        _scoreAnimHandler = new ScoreAnimHandler(_scoreText);
        _countDowner = new CountDowner();
        _settingsUI = GetComponent<SettingsUI>();
        DeactivatePanels();
    }


    private void SlideGameplayUI(bool isOn)
    {
        int multiplier = isOn ? 1 : -1;
        //_gamePanel.transform.DOMoveX()
    }

    private void EndLevelPanel(bool hasWon)
    {
        SetPanelStatus(_gamePanel, false);
        SetGameEndDetails(hasWon);
        SetPanelStatus(_startPanel, true);
    }

    private void DeactivatePanels()
    {
        _startPanel.SetActive(true);
        _startButton.gameObject.SetActive(false);
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(false);
        _settingsPanel.SetActive(false);
    }

    private void GameLoaded()
    {
        _startButton.gameObject.SetActive(true);
        _headerText.text = "";
    }

    private void SetPanelStatus(GameObject panelToSet, bool isActive)
    {
        panelToSet.SetActive(isActive);
    }

    private void SetGameEndDetails(bool hasWon)
    {
        _headerText.text = hasWon ? SUCCESS_TEXT : FAIL_TEXT;
        _startButtonText.text = hasWon ? NEXT_LEVEL_TEXT : RESTART_TEXT;
        _startButton.gameObject.SetActive(true);
    }

    private void RegisterEvents()
    {
        LevelManager.Instance.OnNewLevelLoaded += GameLoaded;
        GameManager.Instance.OnGameEnd += EndLevelPanel;
    }

    private void DeregisterEvents()
    {
        LevelManager.Instance.OnNewLevelLoaded -= GameLoaded;
        GameManager.Instance.OnGameEnd -= EndLevelPanel;
    }
    #endregion
}