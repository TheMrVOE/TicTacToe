using UnityEngine;
using Tools.Patterns.MVC;
using TicTacToe;

public class GameBoardController : BaseController<GameBoardModel>
{

    public override void Init(GameBoardModel model)
    {
        base.Init(model);
    }
    public void LoadMaterials()
    {
        model.matCircle = Resources.Load<Material>(model.matCellCirclePath);
        model.matCross = Resources.Load<Material>(model.matCellCirclePath);
    }

    public CellItem IsEmptyPlaceOnBoard()
    {
        foreach (var item in model.cells)
        {
            if (item.cellType == CellType.None) return item;
        }
        return null;
    }
    public void ClearWeights()
    {
        for (int i = 0; i < model.boardSize; i++)
        {
            for (int j = 0; j < model.boardSize; j++)
            {
                model.weightsCrosses[i, j] = 0;
                model.weightsCircles[i, j] = 0;
            }
        }
    }
    private void ClearDate()
    {
        model.winResult = null;
        model.nowInLine = 0;
    }

    #region CheckForWin
    public WinResult CheckForWinInCell(int cellX, int cellY)
    {
        WinResult result = null;
        CellType myCellType = model.cells[cellX, cellY].cellType;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                result = CheckDirection(cellX, cellY, i, j, myCellType, model.countInLine);
                if (result!=null)
                {
                    Debug.LogWarning("WIN: "+result.cellType);
                    return result;
                }
            }
        }
        return null;
    }
    private WinResult CheckDirection(int x, int y, int dirX, int dirY, CellType myCellType, int length, bool isRecursion = false)
    {
       // Debug.Log("1");
        if(!isRecursion)
        {
            ClearDate();
           // Debug.LogWarning("RECURSION");
        }

        for (int i = 0; i < length; i++)
        {
           // Debug.Log("2");
            int X = x + (i * dirX);
            int Y = y + (i * dirY);

            //On board overflow
            if ((X >= model.boardSize || X < 0 || Y >= model.boardSize || Y < 0))
            {
              //  Debug.Log("3");
               // Debug.LogWarning("OnLimit");
                if (isRecursion)
                {
                    break;
                }

                int newDirX = dirX * -1;
                int newDirY = dirY * -1;

                model.winResult = new WinResult();
                model.winResult.startPoint = new Vector2(X + newDirX, Y + newDirY);

                var result = CheckDirection(x + newDirX, y + newDirY, newDirX, newDirY, myCellType, length - i /*-1*/, true);
                if (result != null)
                {
                  //  Debug.Log("4");
                    return result;
                }
              //  Debug.Log("5");
                break;
            }

          //  Debug.Log("6");
            bool winnerFounded = false;
            if (CheckCell(X, Y, myCellType, out winnerFounded))
            {
              //  Debug.Log("7");
                if (winnerFounded)
                {
                  //  Debug.Log("8");
                    if (model.winResult == null)
                    {
                      //  Debug.Log("9");
                        model.winResult = new WinResult();
                        model.winResult.startPoint = new Vector2(X, Y);
                        model.winResult.endPoint = new Vector2(X + (dirX * -1 * (model.countInLine - 1)), Y + (dirY * -1 * (model.countInLine - 1)));
                    }
                    else
                    {
                        model.winResult.endPoint = new Vector2(X, Y);
                    }
                    model.winResult.cellType = myCellType;
                    return model.winResult;

                }  
            }
            else
            {
               // Debug.Log("10");
               
                if (isRecursion)
                {
                    break;
                }

                int newDirX = dirX * -1;
                int newDirY = dirY * -1;

                model.winResult = new WinResult();
                model.winResult.startPoint = new Vector2(X, Y);

                var result = CheckDirection(x + newDirX, y + newDirY, newDirX, newDirY, myCellType, length - i /*-1*/, true);
                if (result != null)
                {
                   // Debug.Log("101");
                    return result;
                }
                break;
            }
        }
      //  Debug.Log("200");
        return null;
    }
    private bool CheckCell(int X, int Y, CellType type, out bool winnerFounded)
    {
        winnerFounded = false;
        var cellType = model.cells[X, Y].cellType;
        // Debug.LogError("My " + X.ToString() + Y.ToString() + ",  nowInLine: " + model.nowInLine + ", typeOfCell:" + cellType);
        if (cellType != type && cellType != CellType.None)//If is enemies cell
        {
           // Debug.Log("61");
            return false;
        }
        if (cellType == type) //If is my cell
        {
           // Debug.Log("62");
            model.nowInLine++;
            if (model.nowInLine >= model.countInLine)
            {
                winnerFounded = true;
            }
            return true;
        }
        if (cellType == CellType.None) //If is empty cell
        {
           // Debug.Log("63");
            return false;
        }
        return false;
    }
    #endregion
}

public class WinResult
{
    public Vector2 startPoint { get; set; }
    public Vector2 endPoint { get; set; }
    public CellType cellType { get; set; }
}
