using System.Collections.Generic;
using UnityEngine;
using Tools.Patterns.MVC;
using TicTacToe;
using Tools.Patterns.StateMachine;

public class AiModel : BaseModel {

    public const float DELTA_PREFER_PERCENT = 0.2f;

    public Context stateMachine { get; set; }
    public PlayerTypeFigure playerTypeFigure { get; set; }
    public PlayerType playerType { get; set; }

    public List<CellItem> emptyCells = new List<CellItem>();

    public float deltaPreferPercent { get; set; }
    public WaitForSeconds awaitSec { get; set; }

    private GameBoardView _gameBoardView;
    public GameBoardView gameBoardView
    {
        get
        {
            if (_gameBoardView == null)
            {
                _gameBoardView = GameSceneController.instance.gameBoard;
            }
            return _gameBoardView;
        }
    }
    private GameBoardModel _gameBoardModel;
    public GameBoardModel gameBoardModel
    {
        get
        {
            if (_gameBoardModel == null)
            {
                _gameBoardModel = GameSceneController.instance.gameBoard.model;
            }
            return _gameBoardModel;
        }
    }
    private GameSceneController _gameSceneController;
    public GameSceneController gameSceneController
    {
        get
        {
            if(_gameSceneController==null)
            {
                _gameSceneController = GameSceneController.instance;
            }
            return _gameSceneController;
        }
    }


    [HideInInspector]
    public int weight = 0;
    [HideInInspector]
    public int length = 0;
    [HideInInspector]
    public int nowInLine = 0;
    [HideInInspector]
    public int countInLine; // count of item to win

    public void Init(int boardSize)
    {
        base.Init();
        awaitSec = new WaitForSeconds(0.4f);
        countInLine = boardSize;
    }
}
