using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public enum GameType
{
    None,
    Classic,
    Daily
}



public class GameManager : Singleton<GameManager>, IStateManageable
{
    public int score;
    public int highScore;
    public GameType gameType = GameType.None;
    public WordGuessManager wordGuessManager;
    public string currentWord { get; set; } = String.Empty;
    public BaseState currentState { get; private set; }

    public Color backgroundColor;
    
    
    public UnityAction OnNewWord;
    public UnityAction OnGameWon;
    public UnityAction OnGameLost;


    public Dictionary<string, BaseState> States { get; } = new()
    {
        {"intro", new IntroState()},
        {"menu", new MenuState()},
        {"game", new GameState()},
    };

    
    // Start is called before the first frame update
    void Start()
    {
        score = PlayerPrefs.GetInt("Score");
        highScore = PlayerPrefs.GetInt("HighScore");
        SwitchState("menu");
        OnNewWord += RandomColor;
    }

    void RandomColor()
    {
        if (wordGuessManager.pastState == InGameState.Win || wordGuessManager.pastState == InGameState.Loss)
        {
            Camera.main.backgroundColor = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 0.27f, 0.9f);

        }
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void Proceed()
    {
        PopupManager.Instance.CloseCurrentPopup();
        wordGuessManager.Reset();
    }

    public void ResetScore()
    {
        score = 0;
        PlayerPrefs.SetInt("Score", score);
    }

    public void SwitchState(BaseState state)
    {
        currentState?.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }
    
    public void SwitchState(string state)
    {
        currentState?.ExitState(this);
        currentState = States[state];
        currentState.EnterState(this);
    }
    
    public void SetGameType(GameType type)
    {
        gameType = type;
    }
}
