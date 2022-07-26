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
    
    public int NewUser { get; private set; }

    public Color backgroundColor;
    
    
    public UnityAction OnNewWord;
    public UnityAction OnGameWon;
    public UnityAction OnGameLost;
    public UnityAction OnItemBought;
    public UnityAction OnTextChanged;

    //Player Properties
    private int coins;
    private int hints;
    private int eliminations;

    public int interstitialFreq = 2;
    public int GamesWon { get; set; }

    public int CoinsAvailable
    {
        get => coins;
        set
        {
            coins = value;
            PlayerPrefs.SetInt("Coins", coins);
            OnTextChanged?.Invoke();
        }
    }

    public int HintsAvailable
    {
        get => hints;
        set
        {
            hints = value;
            PlayerPrefs.SetInt("Hints", hints);
            OnTextChanged?.Invoke();
        }
    }

    public int EliminationsAvailable
    {
        get => eliminations;
        set
        {
            eliminations = value;
            PlayerPrefs.SetInt("Eliminations", eliminations);
            OnTextChanged?.Invoke();
        }
    }
    
    public int startingCoins = 100;
    public int startingHints = 3;
    public int startingEliminations = 3;

    public int coinsPerGame = 60;
    public int decreasePerRow = 10;
    
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
        NewUser = PlayerPrefs.GetInt("NewUser", 0);
        if (!devMode)
        {
            if (NewUser == 0)
            {
                PlayerPrefs.SetInt("NewUser", 1);
                PlayerPrefs.SetInt("Coins", startingCoins);
                PlayerPrefs.SetInt("Hints", startingHints);
                PlayerPrefs.SetInt("Eliminations", startingEliminations);
            }
            CoinsAvailable = PlayerPrefs.GetInt("Coins");
            HintsAvailable = PlayerPrefs.GetInt("Hints");
            EliminationsAvailable = PlayerPrefs.GetInt("Eliminations");
        }
        else
        {
            CoinsAvailable = 1000;
            HintsAvailable = 10;
            EliminationsAvailable = 10;
        }


        score = PlayerPrefs.GetInt("Score");
        highScore = PlayerPrefs.GetInt("HighScore");
        SwitchState("menu");
        //Gley
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
