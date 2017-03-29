using Tools.Patterns.MVC;
using System;
using Tools.Patterns.StateMachine;

public class AiView : BaseView<AiModel, AiController>, IUser
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

    #region StateMachineHandlers
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
        StartCoroutine(Controller.ExecuteAi());
    }
    public void ActionDone()
    {
        model.gameSceneController.SwitchAnotherMachineState(this as IUser);
    }
    #endregion
}
