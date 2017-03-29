using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Patterns.MVC;
using Tools.Patterns.StateMachine;

public class PlayerModel : BaseModel {

    public Context stateMachine { get; set; }
    public PlayerTypeFigure playerTypeFigure { get; set; }
    public PlayerType playerType { get; set; }

}
