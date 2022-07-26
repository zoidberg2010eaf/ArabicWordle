using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : Page
{
    private int tabActive = 0;
    public Button coinsButton;
    public Button boostersButton;
    public CanvasGroup coinsCanvasGroup;
    public CanvasGroup boostersCanvasGroup;
    
    // Start is called before the first frame update
    void Start()
    {
        coinsButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActiveTab(int tab)
    {
        tabActive = tab;
        if(tab == 0)
        {
            coinsCanvasGroup.alpha = 1;
            boostersCanvasGroup.alpha = 0;
            coinsCanvasGroup.blocksRaycasts = true;
            boostersCanvasGroup.blocksRaycasts = false;
        }
        else
        {
            boostersCanvasGroup.alpha = 1;
            coinsCanvasGroup.alpha = 0;
            boostersCanvasGroup.blocksRaycasts = true;
            coinsCanvasGroup.blocksRaycasts = false;
        }
        //coinsCanvasGroup.alpha = tab == 0 ? 1 : 0;
        //boostersCanvasGroup.alpha = tab == 1 ? 1 : 0;
        coinsButton.interactable = tab == 1;
        boostersButton.interactable = tab == 0;
    }
}
