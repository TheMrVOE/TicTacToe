using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Patterns.MVC;
using TicTacToe;

public class AiController : BaseController<AiModel>
{

    public override void Init(AiModel model)
    {
        base.Init(model);
        model.Init(model.gameBoardModel.boardSize);
        SwitchDificulty();
    }
    private void SwitchDificulty()
    {
        if (GameState.dificultyLevel == DificultyLevel.Two)
        {
            model.deltaPreferPercent = AiModel.DELTA_PREFER_PERCENT;
        }
        else
        {
            model.deltaPreferPercent = 0.0f;
        }
    }

    public IEnumerator ExecuteAi()
    {
        yield return model.awaitSec;

        CalculateWeightOfCells();
        CellItem cell = null;
        FindHardestCells(out cell);
        model.gameSceneController.winResult = model.gameBoardView.Controller.CheckForWinInCell(cell.x, cell.y);

        model.stateMachine.SwitchState();
    }
 
    #region WeightReplacement
    private void CalculateWeightOfCells()
    {
        model.gameBoardView.Controller.ClearWeights();

        var gameBoardModel = model.gameBoardModel;
        for (int x = 0; x < gameBoardModel.boardSize; x++)
        {
            for (int y = 0; y < gameBoardModel.boardSize; y++)
            {
                if (gameBoardModel.cells[x, y].cellType == CellType.Cross)
                {
                    CalcWeightByDirection(x, y, CellType.Cross);
                }
                else if (gameBoardModel.cells[x, y].cellType == CellType.Circle)
                {
                    CalcWeightByDirection(x, y, CellType.Circle);
                }
            }
        }
    }
    private void CalcWeightByDirection(int x, int y, CellType myCellType)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
                CheckDirection(x, y, i, j, myCellType, model.countInLine);
            }
        }
    }
    private void FindHardestCells(out CellItem itemAction)
    {
        itemAction = null;
        var gameBoardModel = model.gameBoardModel;
        int maxWeightMy = 0;
        int maxWeightEnem = 0;

        List<CellItem> myHardestItems = FindHardestCellItems(gameBoardModel.weightsCrosses, out maxWeightMy);
        List<CellItem> enemHardestItems = FindHardestCellItems(gameBoardModel.weightsCrosses, out maxWeightEnem);

        if (!IsOwnTypeCross())
        {
            var t = myHardestItems;
            myHardestItems = enemHardestItems;
            enemHardestItems = t;

            var t2 = maxWeightMy;
            maxWeightMy = maxWeightEnem;
            maxWeightEnem = t2;
        }


        if (maxWeightMy != 0 && maxWeightEnem != 0)
        {

            var itemToDefend = enemHardestItems[Random.Range(0, enemHardestItems.Count)];
            var itemToAttack = myHardestItems[Random.Range(0, myHardestItems.Count)];

            int deltaPrefer = (int)(maxWeightEnem * model.deltaPreferPercent);
            if (maxWeightMy >= maxWeightEnem - deltaPrefer) //Attacking
            {
                //  Debug.Log("Attacking on: " + itemToAttack.name + ", weight:" + maxWeightCrosses);
                itemToAttack.ChangeState(IsOwnTypeCross(), GetOwnMaterial());
                itemAction = itemToAttack;
            }
            else //Defending
            {
                //  Debug.Log("Defending on: " + itemToDefend.name + ", weight:" + maxWeightCircle);
                itemToDefend.ChangeState(IsOwnTypeCross(), GetOwnMaterial());
                itemAction = itemToDefend;
            }
        }
        else
        {
            CellItem item = null;
            if (item = model.gameBoardView.Controller.IsEmptyPlaceOnBoard())
            {
                item.ChangeState(IsOwnTypeCross(), GetOwnMaterial());
                itemAction = item;
            }
        }
    }
    private void CheckDirection(int x, int y, int dirX, int dirY, CellType myCellType, int length, bool isRecursion = false)
    {
        if (!isRecursion)
        {
            ResetData();
        }
        var gameBoardModel = model.gameBoardModel;

        for (int i = 0; i < length; i++)
        {
            int X = x + (i * dirX);
            int Y = y + (i * dirY);

            if ((X >= gameBoardModel.boardSize || X < 0 || Y >= gameBoardModel.boardSize || Y < 0))
            {
                if (isRecursion)
                {
                    return;
                }

                int newDirX = dirX * -1;
                int newDirY = dirY * -1;

                CheckDirection(x + newDirX, y + newDirY, newDirX, newDirY, myCellType, length - i /*-1*/, true);

                return;
            }

            if (!CheckCell(X, Y, myCellType))
            {
                if (isRecursion)
                {
                    break;
                }

                int newDirX = dirX * -1;
                int newDirY = dirY * -1;
                break;
            }

            if (i == length - 1)
            {
                InputWeightsToEmptyCells(myCellType);
            }
        }
    }
    private bool CheckCell(int X, int Y, CellType type)
    {
        var gameBoardModel = model.gameBoardModel;
        var cellType = gameBoardModel.cells[X, Y].cellType;
        if (cellType != type && cellType != CellType.None)//If is enemies cell
        {
            return false;
        }
        if (cellType == type) //If is my cell
        {
            model.nowInLine++;
            model.weight++;
            return true;
        }
        if (cellType == CellType.None) //If is empty cell
        {
            model.emptyCells.Add(gameBoardModel.cells[X, Y]);
            return true;
        }
        return true;
    }
    private void WriteNewWeight(int x, int y, CellType cellType)
    {
        var weight = model.weight;
        var gameBoardModel = model.gameBoardModel;
        var sqr = weight * weight;

        if (cellType == CellType.Cross)
        {
            if (GameState.dificultyLevel == DificultyLevel.One)
            {
                gameBoardModel.weightsCrosses[x, y] += Random.Range(-weight,weight);
            }
            else
            {
                if (gameBoardModel.weightsCrosses[x, y] < sqr) gameBoardModel.weightsCrosses[x, y] = sqr;
            }            
        }
        else
        {
            if (GameState.dificultyLevel == DificultyLevel.One)
            {
                gameBoardModel.weightsCircles[x, y] += Random.Range(-weight, weight);
                    
            }
            else
            {
                if (gameBoardModel.weightsCircles[x, y] < sqr) gameBoardModel.weightsCrosses[x, y] = sqr;
            }
        }
    }
    private void InputWeightsToEmptyCells(CellType cellType)
    {
        var gameBoardModel = model.gameBoardModel;
        var arr = gameBoardModel.weightsCrosses;
        if (cellType != CellType.Cross)
        {
            arr = gameBoardModel.weightsCircles;
        }

        foreach (var item in model.emptyCells)
        {
            WriteNewWeight(item.x, item.y, cellType);
        }
    }
    #endregion

    private List<CellItem> FindHardestCellItems(int[,] arr, out int value)
    {
        List<CellItem> items = new List<CellItem>();
        var gameBoardModel = model.gameBoardModel;

        value = 0;
        for (int i = 0; i < gameBoardModel.boardSize; i++)
        {
            for (int j = 0; j < gameBoardModel.boardSize; j++)
            {
                int v = arr[i, j];
                if (v > value)
                {
                    items.Clear();
                    value = v;
                    items.Add(gameBoardModel.cells[i, j]);
                }
                else
                if (v == value)
                {
                    items.Add(gameBoardModel.cells[i, j]);
                }

                //Debug.Log(gameBoardModel.cells[i, j] + ", weight: " + arr[i, j]);

                //Debug
                // items.Add(cells[i, j], arr[i,j]);
            }
        }
        return items;
    }
    
    private Material GetOwnMaterial()
    {
        return (model.playerTypeFigure == PlayerTypeFigure.Cross ?
                 model.gameSceneController.matCross : model.gameSceneController.matCircle);
    }
    private bool IsOwnTypeCross()
    {
        return model.playerTypeFigure == PlayerTypeFigure.Cross;
    }
    private void ResetData()
    {
        model.weight = 0;
        model.length = model.countInLine;
        model.nowInLine = 0;
        model.emptyCells.Clear();
    }
}
