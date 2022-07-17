using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game : Page
{
    public TextMeshProUGUI topText;
    public TextMeshProUGUI titleText;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnNewWord += ChangeText;
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
}
