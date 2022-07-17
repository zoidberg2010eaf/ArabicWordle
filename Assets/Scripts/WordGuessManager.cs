using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;

public enum InGameState
{
    Typing,
    Win,
    Loss,
    Animation
}

[AddComponentMenu("Word Guess/WordGuessManager")]
public class WordGuessManager : MonoBehaviour
{
    public enum WordMode {
        single,
        array
    }
    public WordMode wordMode = WordMode.array;

    // Single
    public string wordSingle = "BUGGE";

    // Inspector Variables
    public int wordLength = 5;
    public Transform wordGrid;
    // Invoke() - When the word is guessed correctly
    public UnityEvent wordGuessedEvent;
    // Invoke() - When player runs out of guesses
    public UnityEvent wordNotGuessedEvent;
    // Invoke() - When word is too short or if word isn't in the dictionary
    public UnityEvent wordErrorEvent;
    public Color defaultColor = new Color32(18, 18, 19, 255);
    public Color outlineColor = new Color32(63, 63, 63, 255);
    public Color inPlaceColor = new Color32(83, 141, 78, 255);
    public Color inWordColor = new Color32(181, 159, 59, 255);
    public Color notInWordColor = new Color32(42, 42, 42, 255);
    public Sprite defaultWordImage;
    public Sprite wordImage;

    public Color keyboardDefaultColor = new Color(129, 131, 132, 255);
    public Color keyboardDefaultTextColor;
    public Color gridLetterDefaultColor;
    public Color gridLetterCheckedColor;

    private string currentWord = "";
    private string currentWordSimplified = "";
    private string enteredWord = "";
    private int rowIndex = 0;

    private bool wordGuessed, outOfTrials;
    public InGameState CurrentState, pastState;
    public UnityAction<InGameState> OnStateChange;
    

    public Transform keyboard;
    private Dictionary<string, Button> _keyboardButtons = new Dictionary<string, Button>();
    public EnterButton enterButton;
    
    public bool incorrectWord = false;

    private void Awake()
    {
        foreach (Transform row in keyboard.GetChild(0))
        {
            foreach (Button but in row.GetComponentsInChildren<Button>())
            {
                if (but.name == "Enter" || but.name == "Back") continue;
                but.GetComponentInChildren<TextMeshProUGUI>().color = keyboardDefaultTextColor;
                _keyboardButtons.Add(but.name, but);
            }
        }

        foreach (Transform row in wordGrid)
        {
            foreach (Transform letter in row)
            {
                letter.GetComponentInChildren<TextMeshProUGUI>().color = gridLetterDefaultColor;
            }
        }

        //_keyboardButtons.Remove("Enter");
        //_keyboardButtons.Remove("Back");
        OnStateChange += arg0 => print($"Switching to Game State: {arg0.ToString()}");
        //NewWord();
    }

    public void SwitchState(InGameState state)
    {
        switch (state)
        {
            case InGameState.Typing:
                break;
            case InGameState.Win:
                GameWon();
                break;
            case InGameState.Loss:
                GameLost();
                break;
            case InGameState.Animation:
                break;
        }

        pastState = CurrentState;
        CurrentState = state;
        OnStateChange?.Invoke(CurrentState);
    }

    void GameWon()
    {
        GameManager.Instance.score++;
        if(GameManager.Instance.score > GameManager.Instance.highScore)
        {
            GameManager.Instance.highScore = GameManager.Instance.score;
            PlayerPrefs.SetInt("HighScore", GameManager.Instance.highScore);
        }
        PlayerPrefs.SetInt("Score", GameManager.Instance.score);
        SoundManager.Instance.PlayWinSound();
        NotificationsManager.Instance.SpawnNotification(0).onComplete += () =>
        {
            PopupManager.Instance.OpenPopup(1);
            GameManager.Instance.OnGameWon?.Invoke();
        };
        //PopupManager.Instance.OpenPopup(1);
        //GameManager.Instance.OnGameWon?.Invoke();
        PlayerPrefs.Save();
    }

    void GameLost()
    {
        //GameManager.Instance.score = 0;
        NotificationsManager.Instance.SpawnNotification(1).onComplete += () =>
        {
            PopupManager.Instance.OpenPopup(2);
            GameManager.Instance.OnGameLost?.Invoke();
            GameManager.Instance.ResetScore();
        };
        //PopupManager.Instance.OpenPopup(2);
        //GameManager.Instance.OnGameLost?.Invoke();
    }

    public void NewWord()
    {
        // Single: Gives you the word set in the Inspector
        if(wordMode == WordMode.single) currentWord = wordSingle;
        // Array: Gives you a random word from the dictionary
        else
        {
            int index = Random.Range(0, WordArray.WordList.Length);
            currentWord = WordArray.WordList[index];
            
        }
        currentWordSimplified = currentWord;
        currentWordSimplified = Regex.Replace(currentWordSimplified, @"[أ|إ|آ]", "ا");
        currentWordSimplified = Regex.Replace(currentWordSimplified, @"[ى]", "ي");
    
        SwitchState(InGameState.Typing);
        GameManager.Instance.currentWord = currentWord;
        GameManager.Instance.OnNewWord?.Invoke();
    }

    private bool WordNotInDictionary()
    {
        return (!WordArray.AllWordsDict.ContainsKey(enteredWord[0].ToString()) ||
            System.Array.IndexOf(WordArray.AllWordsDict[enteredWord[0].ToString()], enteredWord) == -1);
    }

    public void EnterLetter(string str)
    {
        // \b is backspace (delete character) and \n is enter (new line)
        // Converting string parts to charcters
        str = str.Replace("Back", "\b").Replace("Enter", "\n");

        if (CurrentState == InGameState.Typing)
        {
            foreach (char c in str)
            {
                // Removes character from end of string
                if (c == '\b' && enteredWord.Length > 0) enteredWord = enteredWord.Substring(0, enteredWord.Length - 1);
                // Submits word for validation
                else if (c == '\n' || c == '\r')
                {
                    // Checks if word is too short
                    if (enteredWord.Length != wordLength)
                    {
                        wordErrorEvent.Invoke();
                        return;
                    }

                    // Checks if word is in dictionary
                    // Check for word here
                    if (incorrectWord){
                        //wordGrid.GetChild(rowIndex).DOShakePosition(0.5f, 100);
                        StartCoroutine(Shake(rowIndex, 1));
                        wordErrorEvent.Invoke();
                        return;
                    }

                    // Checks and colors the current row
                    CheckRow();
                    // Checks if the word was guessed correctly or whether there's no guesses left
                    if (enteredWord == currentWordSimplified || rowIndex + 1 >= wordGrid.childCount)
                    {
                        if (enteredWord == currentWordSimplified)
                        {
                            wordGuessed = true;
                            wordGuessedEvent.Invoke();
                        }
                        else if (rowIndex + 1 >= wordGrid.childCount)
                        {
                            outOfTrials = true;
                            wordNotGuessedEvent.Invoke();
                        }

                        //StartCoroutine(ResetTimeout(5));
                        return;
                    }

                    // Jump to next row
                    rowIndex++;
                    enteredWord = "";
                }
                else
                {
                    enteredWord += c;
                }

                enteredWord = ValidateWord(enteredWord);
                enterButton.SetInteractable(enteredWord.Length == 5);
                if (enteredWord.Length == 5)
                {
                    incorrectWord = WordNotInDictionary();
                    enterButton.SetIncorrectWord(incorrectWord);
                }

                DisplayWord();
                SoundManager.Instance.PlayClickSound();
            }
        }
    }

    public void DisplayWord()
    {
        Transform row = wordGrid.GetChild(rowIndex);
        for(int i = 0; i < row.childCount; i++)
        {
            string str = enteredWord.Length > i ? enteredWord[i].ToString() : "";
            /*if (str == "ي" && i != row.childCount - 1)
            {
                str = "يـ";
            }else if(str == "ئ" && i != row.childCount - 1)
            {
                str = "ئـ";
            }*/
            
            row.GetChild(row.childCount - i - 1).GetComponentInChildren<TextMeshProUGUI>().text = str;
        }
    }

    public string ValidateWord(string str)
    {
        if(str == "" || str == null) return "";
        // Sets length of string if it's too long
        if(str.Length > wordLength) str = str.Substring(0, wordLength);
        // Remove anything else than letters
        str = Regex.Replace(str, @"[^\u0600-\u06ff]", "");

        //str = str.ToUpper();
        return str;
    }
    
    public void CheckRow()
    {
        List<Color> colors = new List<Color>();
        List<int> notInRightPlaceIndices = new();
        Transform row = wordGrid.GetChild(rowIndex);
        List<Image> notInRightPlaceImages = new List<Image>();
        List<char> notInRightPlaceChars = new List<char>();
        string letterCount = currentWordSimplified;
        for(int i = 0; i < row.childCount; i++)
        {
            Image img = row.GetChild(row.childCount - i - 1).GetComponent<Image>();
            if(enteredWord[i].ToString() == currentWordSimplified[i].ToString())
            {
                Regex regex = new Regex(Regex.Escape(currentWordSimplified[i].ToString()));
                Image buttonImg = _keyboardButtons[enteredWord[i].ToString()].gameObject.GetComponent<Image>();
                letterCount = regex.Replace(letterCount, "", 1);
                //img.color = inPlaceColor;
                buttonImg.color = inPlaceColor;
                buttonImg.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                colors.Add(inPlaceColor);
            } else
            {
                notInRightPlaceImages.Add(img);
                notInRightPlaceChars.Add(enteredWord[i]);
                notInRightPlaceIndices.Add(i);
                colors.Add(notInWordColor);
            }
        }

        for(int i = 0; i < notInRightPlaceImages.Count; i++)
        {
            Image img = notInRightPlaceImages[i];
            Image buttonImg = _keyboardButtons[notInRightPlaceChars[i].ToString()].gameObject.GetComponent<Image>();
            if(letterCount.Contains(notInRightPlaceChars[i])) 
            {
                Regex regex = new Regex(Regex.Escape(notInRightPlaceChars[i].ToString()));
                letterCount = regex.Replace(letterCount, "", 1);
                //img.color = inWordColor;
                colors[notInRightPlaceIndices[i]] = inWordColor;
                buttonImg.color = (buttonImg.color == inPlaceColor) ? inPlaceColor : inWordColor;
            }
            else
            {
                //img.color = notInWordColor;
                buttonImg.color = (buttonImg.color == inPlaceColor) ? inPlaceColor : (buttonImg.color == inWordColor) ? inWordColor : notInWordColor;
            }
            buttonImg.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }

        SwitchState(InGameState.Animation);
        Sequence seq = DOTween.Sequence();
        Tweener t = row.GetChild(4).DOLocalRotate(new Vector3(90, 0, 0), 0.1f);
        Image ims = row.GetChild(4).GetComponent<Image>();
        t.onComplete += () =>
        {
            ims.sprite = wordImage;
            ims.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = gridLetterCheckedColor;
        };
        seq.Append(t);
        seq.Append(DOTween.To(() => ims.color, x => ims.color = x, colors[0], 0.1f).SetDelay(0.1f));
        seq.Join(row.GetChild(4).DOLocalRotate(new Vector3(0, 0, 0), 0.1f));
        
        for(int i = 1; i < 5; i++)
        {
            Image im = row.GetChild(5-i-1).GetComponent<Image>();
            //seq.AppendInterval(0.05f);
            Tweener t2 = row.GetChild(5 - i - 1).DOLocalRotate(new Vector3(90, 0, 0), 0.1f);
            t2.onComplete += () =>
            {
                im.sprite = wordImage;
                im.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = gridLetterCheckedColor;
            };
            seq.Join(t2.SetDelay(0.05f));
            seq.Join(DOTween.To(() => im.color, x => im.color = x, colors[i], 0.1f).SetDelay(0.1f));
            if (i == 4 && row.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text == "ﻱ" && currentWord[^1] == 'ى')
            {
                seq.Join(row.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().DOText("ى", 0.1f));
            }
            seq.Join(row.GetChild(5 - i - 1).DOLocalRotate(new Vector3(0, 0, 0), 0.1f));
        }

        seq.onComplete += () =>
        {
            if (wordGuessed)
            {
                SwitchState(InGameState.Win);
            }else if (outOfTrials)
            {
                SwitchState(InGameState.Loss);
            }
            else
            {
                SwitchState(InGameState.Typing);
            }
        };
    }

    public IEnumerator ResetTimeout(float timeout)
    {
        yield return new WaitForSeconds(timeout);
        Reset();
    }

    public void Reset()
    {
        if(!wordGrid) return;
        // Gets all characters displayed in the grid
        TextMeshProUGUI[] gridTMPro = wordGrid.GetComponentsInChildren<TextMeshProUGUI>();
        // Gets all boxes behind the characters
        Image[] images = wordGrid.GetComponentsInChildren<Image>();

        // Resets box colors
        foreach(Image image in images) image.color = defaultColor;
        // Resets characters
        foreach(TextMeshProUGUI tmPro in gridTMPro) tmPro.text = "";

        foreach (var btn in _keyboardButtons.Values)
        {
            btn.GetComponent<Image>().color = keyboardDefaultColor;
        }
        // Jumps to first row
        rowIndex = 0;
        enteredWord = "";
        wordGuessed = outOfTrials = false;
        
        foreach (Transform row in wordGrid)
        {
            foreach (Transform letter in row)
            {
                letter.GetComponent<Image>().sprite = defaultWordImage;
                letter.GetComponentInChildren<TextMeshProUGUI>().color = gridLetterDefaultColor;
            }
        }
        
        foreach (Transform row in keyboard.GetChild(0))
        {
            foreach (Button but in row.GetComponentsInChildren<Button>())
            {
                if (but.name == "Enter" || but.name == "Back") continue;
                but.GetComponentInChildren<TextMeshProUGUI>().color = keyboardDefaultTextColor;
            }
        }
        
        NewWord();
    }


    IEnumerator Shake(int row, float duration)
    {
        float startTime = Time.time;
        float time = Time.time - startTime;
        while (time <= duration)
        {
            float x = 50*Mathf.Sin(40 * time) * Mathf.Exp(-5 * time);
            wordGrid.GetChild(row).localPosition = new Vector3(x, wordGrid.GetChild(row).localPosition.y, wordGrid.GetChild(row).localPosition.z);
            yield return new WaitForSeconds(0.01f);
            time = Time.time - startTime;
        }
    }


#if UNITY_EDITOR
    private void OnValidate() {
        if(!wordGrid) return;
        Image[] images = wordGrid.GetComponentsInChildren<Image>();
        foreach(Image image in images) image.color = defaultColor;

        Outline[] outlines = wordGrid.GetComponentsInChildren<Outline>();
        foreach(Outline outline in outlines) outline.effectColor = outlineColor;
    }
#endif
}