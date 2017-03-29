using Tools.Patterns.StateMachine;

public class ActionMachineState : State {

    public override void Handle(Context context)
    {
        base.Handle(context);
        context.state = new AwaitMachineState();
    }
}
