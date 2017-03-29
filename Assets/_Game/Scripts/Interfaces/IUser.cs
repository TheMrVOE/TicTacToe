using Tools.Patterns.StateMachine;

public interface IUser {

    Context stateMachine { get; set; }
    PlayerTypeFigure playerTypeFigure { get; set; }
    PlayerType playerType { get; set; }
    void Action();
    void ActionDone();

}
