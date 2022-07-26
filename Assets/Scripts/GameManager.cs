using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
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
    public bool devMode = false;
    public int score;
    public int highScore;
    public GameType gameType = GameType.None;
    public WordGuessManager wordGuessManager;
    public string CurrentWord { get; set; } = String.Empty;
    public string CurrentWordSimplified { get; set; } = String.Empty;
    public BaseState CurrentState { get; private set; }

    public Color backgroundColor;
    
    
    public UnityAction OnNewWord;
    public UnityAction OnGameWon;
    public UnityAction OnGameLost;

    //Player Properties
    public int CoinsAvailable { get; set; } = 0;
    public int HintsAvailable { get; set; } = 6;
    public int EliminationsAvailable { get; set; } = 6;
    
    public int hintLimit = 3;
    [HideInInspector] public int timesHintUsed = 0;
    public int eliminationLimit = 3;
    [HideInInspector] public int timesEliminationUsed = 0;


    
    public Dictionary<string, BaseState> States { get; } = new()
    {
        {"intro", new IntroState()},
        {"menu", new MenuState()},
        {"game", new GameState()},
        {"store", new StoreState()}
    };

    
    // Start is called before the first frame update
    void Start()
    {
        
        score = PlayerPrefs.GetInt("Score");
        highScore = PlayerPrefs.GetInt("HighScore");
        SwitchState("menu");
        //Gley
        //Advertisements.Instance.Initialize();
        //Advertisements.Instance.ShowBanner(BannerPosition.BOTTOM, BannerType.SmartBanner);
        AdsManager.Instance.InitializeAds();
        OnNewWord += RandomColor;
        AdsManager.Instance.LoadBanner();
        //AdsManager.Instance.ShowBanner();
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
        CurrentState.UpdateState(this);
        print(CurrentState.stateName);
    }

    public void Proceed()
    {
        PopupManager.Instance.CloseCurrentPopup();
        timesEliminationUsed = timesHintUsed = 0;
        wordGuessManager.Reset();
    }

    public void ResetScore()
    {
        score = 0;
        PlayerPrefs.SetInt("Score", score);
    }

    public void SwitchState(BaseState state)
    {
        CurrentState?.ExitState(this);
        CurrentState = state;
        CurrentState.EnterState(this);
    }
    
    public void SwitchState(string state)
    {
        CurrentState?.ExitState(this);
        CurrentState = States[state];
        CurrentState.EnterState(this);
    }
    
    public void SetGameType(GameType type)
    {
        gameType = type;
    }
}
