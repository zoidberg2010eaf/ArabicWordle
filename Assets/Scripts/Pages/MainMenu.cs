using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenu : Page
{
    public Button playClassicButton;
    public Button playDailyButton;
    public Button settingsButton;
    public TextMeshProUGUI highScore;
    public TextMeshProUGUI coinsText;

    private void Start()
    {
        highScore.text = GameManager.Instance.highScore.ToString();
        coinsText.text = GameManager.Instance.CoinsAvailable.ToString();
        GameManager.Instance.OnTextChanged += SetText;
    }
    
    void SetText()
    {
        coinsText.DOText(GameManager.Instance.CoinsAvailable.ToString(), 0.25f);
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
