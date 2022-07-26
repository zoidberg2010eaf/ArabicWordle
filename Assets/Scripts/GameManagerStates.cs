using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroState : BaseState
{
    public override void EnterState(IStateManageable stateManager)
    {
    }

    public override void UpdateState(IStateManageable stateManager)
    {
    }

    public override void ExitState(IStateManageable stateManager)
    {
    }

    public IntroState() : base("Intro")
    {
    }
}

public class MenuState : BaseState
{
    public override void EnterState(IStateManageable stateManager)
    {
    }

    public override void UpdateState(IStateManageable stateManager)
    {
    }

    public override void ExitState(IStateManageable stateManager)
    {
    }
    
    public MenuState() : base("Menu")
    {
    }
}

public class StoreState : BaseState
{
    public override void EnterState(IStateManageable stateManager)
    {
    }

    public override void UpdateState(IStateManageable stateManager)
    {
    }

    public override void ExitState(IStateManageable stateManager)
    {
    }
    
    public StoreState() : base("Store")
    {
    }
}


public class GameState : BaseState
{
    //public InGameState CurrentState { get; set; } = InGameState.Typing;
    public override void EnterState(IStateManageable stateManager)
    {
        if (GameManager.Instance.CurrentWord.Length == 5)
        {
            return;
        }
        GameManager.Instance.wordGuessManager.NewWord();
    }

    public override void UpdateState(IStateManageable stateManager)
    {
        
    }

    public override void ExitState(IStateManageable stateManager)
    {
    }
    
    public GameState() : base("Game")
    {
    }
}
