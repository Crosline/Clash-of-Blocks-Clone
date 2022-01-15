using _Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    [SerializeField]
    private GameObject _gameCanvas;
    [SerializeField]
    private Text _levelText;
    [SerializeField]
    private GameObject _winCanvas;
    [SerializeField]
    private GameObject _loseCanvas;

    public GameState State { get; private set; }

    [SerializeField]
    private InputHandler _inputHandler;

    [SerializeField]
    private int _level = 0;
    public List<Level> levels = new List<Level>();

    void Start() {

        if (_gameCanvas == null)
            _gameCanvas = GameObject.Find("GameCanvas");


        if (_levelText == null)
            _levelText = GameObject.Find("LevelText").GetComponent<Text>();


        if (_winCanvas == null)
            _winCanvas = GameObject.Find("WinCanvas");


        if (_loseCanvas == null)
            _loseCanvas = GameObject.Find("LoseCanvas");



        if (_inputHandler == null)
            _inputHandler = GetComponent<InputHandler>();


        ChangeState(GameState.Starting);
    }

    public void ChangeState(GameState newState) {
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState) {
            case GameState.Starting:
                Init();
                break;
            case GameState.WaitingInput:
                WaitingInput();
                break;
            case GameState.Running:
                Running();
                break;
            case GameState.Win:
                Win();
                break;
            case GameState.Lose:
                Lose();
                break;
            case GameState.Restart:
                Restart();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);

        Debug.Log($"New State: {newState}");
    }
    #region State Functions

    private void Init() {

        ResetCanvases();

        GridManager.Instance.GridSetup(levels[_level]);

        ChangeState(GameState.WaitingInput);
    }

    private void WaitingInput() {

        _inputHandler.isActive = true;

    }

    private void Running() {

        GridManager.Instance.StartRun();

    }
    private void Restart() {
        ResetCanvases();

        GridManager.Instance.CleanGrid();
        ChangeState(GameState.Starting);
    }

    private void Win() {
        _gameCanvas.SetActive(false);
        _winCanvas.SetActive(true);
    }

    private void Lose() {
        _gameCanvas.SetActive(false);
        _loseCanvas.SetActive(true);
    }

    #endregion

    private void ResetCanvases() {
        _winCanvas.SetActive(false);
        _loseCanvas.SetActive(false);

        _gameCanvas.SetActive(true);
        _levelText.text = $"Level {_level + 1}";

        GameObject.Find("PlayerPercentage").GetComponent<Text>().text = $"";
        GameObject.Find("EnemyPercentage").GetComponent<Text>().text = $"";
        GameObject.Find("Enemy2Percentage").GetComponent<Text>().text = $"";
    }

    #region Button Functions

    public void RestartGame() {
        ChangeState(GameState.Restart);
    }



    public void NextGame() {
        _level++;

        if (_level == levels.Count) {
            _level = 0;
        }

        ChangeState(GameState.Restart);
    }

    #endregion

    [Serializable]
    public enum GameState {
        Starting = 0,
        WaitingInput = 1,
        Running = 2,
        Win = 3,
        Lose = 4,
        Restart = 5,
    }
}
