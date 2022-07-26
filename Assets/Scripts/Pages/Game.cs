using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game : Page
{
    public TextMeshProUGUI topText;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI coinsText;

    public HintButton hintButton;
    public EliminateButton eliminateButton;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnNewWord += ChangeText;
        GameManager.Instance.OnNewWord += () => print(GameManager.Instance.CurrentWord);
        coinsText.text = GameManager.Instance.CoinsAvailable.ToString();
        hintButton.SetCounter();
        eliminateButton.SetCounter();
        GameManager.Instance.OnTextChanged += SetText;
    }

    void SetText()
    {
        coinsText.DOText(GameManager.Instance.CoinsAvailable.ToString(), 0.25f);
    }

    
    // Update is called once per frame
    void ChangeText()
    {
        if (GameManager.Instance.gameType == GameType.Classic)
        {
            topText.text = "عدد النقاط";
            titleText.text = GameManager.Instance.score.ToString();
        }
        
    }
    
    private void OnEnable()
    {
        //GameManager.Instance.SwitchState("game");
    }
}
