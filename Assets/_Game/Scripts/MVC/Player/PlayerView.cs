using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Patterns.MVC;
using System;
using Tools.Patterns.StateMachine;
using TicTacToe;

public class PlayerView : BaseView<PlayerModel, PlayerController>, IUser
{
    public Context stateMachine
    {
        get
        {
            return model.stateMachine;
        }
        set
        {
            model.stateMachine = value;
        }
    }
    public PlayerTypeFigure playerTypeFigure
    {
        get
        {
            return model.playerTypeFigure;
        }
        set
        {
            model.playerTypeFigure = value;
        }
    }
    public PlayerType playerType
    {
        get
        {
            return model.playerType;
        }
        set
        {
            model.playerType = value;
        }
    }

    public override void Awake()
    {
        base.Awake();
        stateMachine = new Context(new AwaitMachineState());
        stateMachine.onStateChanged += OnStateChanged;
    }

    #region StateMAchineHandlers
    public void OnStateChanged(State newState)
    {
        if (newState.GetType() == typeof(ActionMachineState))
        {
            Action();
        }
        else if (newState.GetType() == typeof(AwaitMachineState))
        {
            ActionDone();
        }
    }
    public void Action()
    {
        
    }
    public void ActionDone()
    {
        GameSceneController.instance.SwitchAnotherMachineState(this as IUser);
    }
    #endregion

    private void Update()
    {
        if (stateMachine.state.GetType() == typeof(ActionMachineState))
        {
            HandleInput();
        }
    }





    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                CellItem item;
                if (item = hit.transform.GetComponent<CellItem>())
                {
                    Material myMat = (model.playerTypeFigure == PlayerTypeFigure.Cross ?
                    GameSceneController.instance.matCross : GameSceneController.instance.matCircle);

                    item.GetComponent<CellItem>().ChangeState(model.playerTypeFigure == PlayerTypeFigure.Cross, myMat);

                    GameSceneController.instance.winResult = GameSceneController.instance.gameBoard.Controller.CheckForWinInCell(item.x, item.y);
                    stateMachine.SwitchState();
                }
            }
        }
    }
}
