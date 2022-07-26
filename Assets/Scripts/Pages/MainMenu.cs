using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Page
{
    public Button playClassicButton;
    public Button playDailyButton;
    public Button settingsButton;
    public TextMeshProUGUI highScore;

    private void Start()
    {
        highScore.text = GameManager.Instance.highScore.ToString();
    }

    public void PlayClassic()
    {
        GameManager.Instance.SetGameType(GameType.Classic);
        GameManager.Instance.SwitchState(GameManager.Instance.States["game"]);
    }
    
    public void PlayDaily()
    {
        GameManager.Instance.SetGameType(GameType.Daily);
        GameManager.Instance.SwitchState(GameManager.Instance.States["game"]);
    }
    
    private void OnEnable()
    {
        //GameManager.Instance.SwitchState("menu");
    }
}
