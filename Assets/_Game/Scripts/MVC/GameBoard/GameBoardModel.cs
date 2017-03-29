using UnityEngine;
using Tools.Patterns.MVC;
using TicTacToe;

[System.Serializable]
public class GameBoardModel : BaseModel {

    public string cellItemPath = "Prefabs/CellItem";
    public string matCellCrossPath = "Materials/MatCellCross";
    public string matCellCirclePath = "Materials/MatCellCircle";

    public Material matCircle;
    public Material matCross;

    [HideInInspector]
    public CellItem[,] cells;

    [HideInInspector]
    public int boardSize;
    [HideInInspector]
    public int countInLine;

    public WinResult winResult { get; set; }
    public int nowInLine;

    public int[,] weightsCrosses;
    public int[,] weightsCircles;
}
