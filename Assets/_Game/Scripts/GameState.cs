using TicTacToe;

public enum PlayerType { Player = 0, CPU = 1 }
public enum DificultyLevel { One, Two, Three }
public enum PlayerTypeFigure { Cross, Circle}
public enum WinResultEnum { Player1, Player2, Draw}

public static class GameState {
#if UNITY_EDITOR
    public static bool isInitializad = false;
#endif
    public const int BOARD_MAX_SIZE = 5;
    public const int BOARD_MIN_SIZE = 3;

    public static PlayerType[] playerTypes;

    public static bool isPlayer1Turn = true;

    public static int boardSize { get; set; }

    public static int player1WinCount;
    public static int player2WinCount;

    public static DificultyLevel dificultyLevel = DificultyLevel.Two;
}
