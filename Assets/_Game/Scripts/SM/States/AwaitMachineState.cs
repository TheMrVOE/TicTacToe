using Tools.Patterns.StateMachine;

public class AwaitMachineState : State {

    public override void Handle(Context context)
    {
        base.Handle(context);
        context.state = new ActionMachineState();
    }
}
