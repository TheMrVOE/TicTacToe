using UnityEngine;
using Tools.Patterns.MVC;
using TicTacToe;

public class GameBoardView : BaseView<GameBoardModel, GameBoardController>
{
    public override void Awake()
    {
        base.Awake();
        model.boardSize = GameState.boardSize;
        model.countInLine = model.boardSize;
        Controller.LoadMaterials();
        GenerateGameBoard();
    }

    public void GenerateGameBoard()
    {
        model.cells = new CellItem[model.boardSize, model.boardSize];
        var prefab = Resources.Load<GameObject>(model.cellItemPath);
        for (int r = 0; r < model.boardSize; r++)
        {
            for (int c = 0; c < model.boardSize; c++)
            {
                Vector3 pos;
                pos.x = r;
                pos.y = 0;
                pos.z = c;

                model.cells[r, c] = Instantiate(prefab).GetComponent<CellItem>();
                var item = model.cells[r, c];
                item.transform.position = pos;
                item.transform.name = "cell_" + r + c;
                item.GetOwnIndex(r, c);
            }
        }

        model.weightsCrosses = new int[model.boardSize, model.boardSize];
        model.weightsCircles = new int[model.boardSize, model.boardSize];
    }
}
