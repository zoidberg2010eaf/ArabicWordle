using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LossPopup : Popup
{
    public TextMeshProUGUI score, highScore;
    // Start is called before the first frame update
    void Awake()
    {
        GameManager.Instance.OnGameLost += ChangeScore;
    }

    private void ChangeScore()
    {
        score.text = GameManager.Instance.score.ToString();
        highScore.text = GameManager.Instance.highScore.ToString();
    }
}
