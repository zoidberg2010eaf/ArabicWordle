using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game : Page
{
    public TextMeshProUGUI topText;
    public TextMeshProUGUI titleText;

    public HintButton hintButton;
    public EliminateButton eliminateButton;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnNewWord += ChangeText;
        GameManager.Instance.OnNewWord += () => print(GameManager.Instance.CurrentWord);
        hintButton.SetCounter();
        eliminateButton.SetCounter();
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
