using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinPopup : Popup
{
    public TextMeshProUGUI currentWord;

    public TextMeshProUGUI score, highScore;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnNewWord += ChangeWord;
        GameManager.Instance.OnGameWon += ChangeScore;
        
    }

    private void ChangeWord()
    {
        currentWord.text = GameManager.Instance.CurrentWord;
    }

    private void ChangeScore()
    {
        score.text = GameManager.Instance.score.ToString();
        highScore.text = GameManager.Instance.highScore.ToString();
    }
}
