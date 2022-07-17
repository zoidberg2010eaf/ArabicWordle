using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PagesManager : Singleton<PagesManager>
{
    public List<Page> pages = new();
    public Page currentPage;
    public Page previousPage;
    // Start is called before the first frame update
    
    void Start()
    {
        pages.ForEach(page => page.gameObject.SetActive(false));
        currentPage = pages[0];
        currentPage.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void FlipPage(Page page)
    {
        if (currentPage != null)
        {
            previousPage = currentPage;
        }
        currentPage = page;
        currentPage.OpenPage();
        previousPage.ClosePage();
    }
    
    public void FlipPage(int pageIndex)
    {
        FlipPage(pages[pageIndex]);
    }
    
    public void GoBack()
    {
        if(GameManager.Instance.currentState == GameManager.Instance.States["game"])
        {
            GameManager.Instance.SwitchState("menu");
        }
        else
        {
            FlipPage(previousPage);
        }
        FlipPage(previousPage);
    }
}
