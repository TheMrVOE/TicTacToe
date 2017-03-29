using System.Collections;
using UnityEngine;
using TicTacToe;

public class GameSceneController : MonoBehaviour
{
    public static GameSceneController instance { get; private set; }
    [HideInInspector]
    public GameBoardView gameBoard;

    [SerializeField]
    private ControllerUiGame _controllerUiGame;

    [SerializeField]
    private LineRenderer _lineRenderer;

    [SerializeField] private GameObject _gameBoardViewPref;
    public Material matCross;
    public Material matCircle;

    public bool isFirstTurn { get; set; }

    private IUser[] users = new IUser[2];
    [HideInInspector]
    public bool isWinnerFounded = false;
    public WinResult winResult { get; set; }

    private void Awake()
    {
#if UNITY_EDITOR
        if(!GameState.isInitializad)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("IntoScene");
            return;
        }
#endif
        isFirstTurn = true;
        InitSingletone();
        InitObjects();
        ReplaceMainCamera();
        _controllerUiGame.ResetValues(GameState.player1WinCount, GameState.player2WinCount);
        StartMachines();
    }
    public void SwitchAnotherMachineState(IUser caller)
    {
        CheckForGameResult();

        foreach (var user in users)
        {
            if (user != caller)
            {
                user.stateMachine.SwitchState();
                _controllerUiGame.SetImagePlayerTurnState(users[0]==user);
                break;
            }
        }
    }

    private void InitSingletone()
    {
        instance = this;
    }
    private void InitObjects()
    {
        InitGameBoard();
        InitUsers();
    }
    private void InitGameBoard()
    {
        gameBoard = Instantiate(_gameBoardViewPref).GetComponent<GameBoardView>();
    }
    private void InitUsers()
    {
        var players = GameState.playerTypes;
        for (int i = 0; i < players.Length; i++)
        {
            var go = new GameObject("User" + i);

            if (players[i] == PlayerType.CPU)
            {
                users[i] = go.AddComponent<AiView>() as IUser;
            }
            else
            {
                users[i] = go.AddComponent<PlayerView>() as IUser;
            }
        }

        if (GameState.isPlayer1Turn)
        {
            users[0].playerTypeFigure = PlayerTypeFigure.Cross;
            users[1].playerTypeFigure = PlayerTypeFigure.Circle;
        }
        else
        {
            users[0].playerTypeFigure = PlayerTypeFigure.Circle;
            users[1].playerTypeFigure = PlayerTypeFigure.Cross;
        }
        _controllerUiGame.SetImagePlayerTurnState(GameState.isPlayer1Turn);

    }
    private void ReplaceMainCamera()
    {
        Vector3 avarage = Vector3.zero;
        float i = 0;
        foreach (var item in gameBoard.model.cells)
        {
            avarage += item.transform.position;
            i++;
        }
        Camera.main.transform.position = new Vector3(avarage.x / i, 8, avarage.z / i);
        Camera.main.orthographicSize = (gameBoard.model.boardSize / 2.0f) * 1.75f;
    }
    private void StartMachines()
    {
        if (GameState.isPlayer1Turn)
        {
            users[0].stateMachine.SwitchState();
        }
        else
        {
            users[1].stateMachine.SwitchState();
        }
    }

    private void CheckForGameResult()
    {
        var winResult = gameBoard.model.winResult;
        var isWin = (winResult != null && winResult.cellType != CellType.None);
        if (isWin)
        {
            HandleWin(gameBoard.model.winResult);
        }
        else if (!gameBoard.Controller.IsEmptyPlaceOnBoard())
        {
            HandleDraw();
        }
    }
    private void HandleDraw()
    {
        foreach (var user in users)
        {
            user.stateMachine.SetState(new IdleMachineState());
        }

        GameState.isPlayer1Turn = !GameState.isPlayer1Turn;
        StartCoroutine(RoutineAwaitShow());
    }
    private void HandleWin(WinResult winResult)
    {
        foreach (var user in users)
        {
            user.stateMachine.SetState(new IdleMachineState());
        }

        var winRes = gameBoard.model.winResult;
        _lineRenderer.SetPosition(0, new Vector3(winRes.startPoint.x, 0.5f, winRes.startPoint.y));
        _lineRenderer.SetPosition(1, new Vector3(winRes.endPoint.x, 0.5f, winRes.endPoint.y));

        if (winResult.cellType == CellType.Cross)
        {
            if (users[0].playerTypeFigure == PlayerTypeFigure.Cross)
            {
                GameState.isPlayer1Turn = true;
                GameState.player1WinCount++;
            }
            else
            {
                GameState.isPlayer1Turn = false;
                GameState.player2WinCount++;
            }
        }
        else
        {
            if (users[1].playerTypeFigure == PlayerTypeFigure.Circle)
            {
                GameState.isPlayer1Turn = false;
                GameState.player2WinCount++;
            }
            else
            {
                GameState.isPlayer1Turn = true;
                GameState.player1WinCount++;
            }
        }

        StartCoroutine(RoutineAwaitShow(winResult));
    }
    private IEnumerator RoutineAwaitShow(WinResult winResult = null)
    {
        yield return new WaitForSeconds(0.5f);

        WinResultEnum result = WinResultEnum.Draw;

        if (winResult != null)
        {
            if (winResult.cellType == CellType.Cross)
            {
                result = (users[0].playerTypeFigure == PlayerTypeFigure.Cross ? result = WinResultEnum.Player1 : result = WinResultEnum.Player2);
            }
            else
            {
                result = (users[0].playerTypeFigure == PlayerTypeFigure.Circle ? result = WinResultEnum.Player1 : result = WinResultEnum.Player2);
            }
        }
        _controllerUiGame.SetGameResult(result);
    }
}
