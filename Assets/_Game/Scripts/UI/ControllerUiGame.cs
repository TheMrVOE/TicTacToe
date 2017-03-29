using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TicTacToe
{
    public class ControllerUiGame : MonoBehaviour
    {
        [SerializeField]
        private Button _btnMenu;
        [SerializeField]
        private TextMeshProUGUI _txtPlayer1;
        [SerializeField]
        private TextMeshProUGUI _txtPlayer2;
        [SerializeField]
        private TextMeshProUGUI _txtWinResult;
        [SerializeField]
        private TextMeshProUGUI _txtPlayerWinResult;
        [SerializeField]
        private Animator _anmtrGameResult;
        [SerializeField]
        private Button _btnPlayAgain;
        [SerializeField]
        private Animator _anmtrPlayerTurn;

        private void Start()
        {
            AddListeners();
        }

        private void AddListeners()
        {
            _btnMenu.onClick.AddListener(() =>
            {
                ClearSessionData();
                UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
            });

            _btnPlayAgain.onClick.AddListener(() =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
            });
        }

        public void SetImagePlayerTurnState(bool isPlayer1)
        {
            if(isPlayer1)
            {
                _anmtrPlayerTurn.SetTrigger("PlayP1");
            }
            else
            {
                _anmtrPlayerTurn.SetTrigger("PlayP2");
            }
        }
        public void SetGameResult(WinResultEnum winResult)
        {
            if (winResult == WinResultEnum.Draw)
            {
                _txtPlayerWinResult.gameObject.SetActive(false);
                _txtWinResult.text = "DRAW !";
            }
            else
            {
                _txtPlayerWinResult.gameObject.SetActive(true);
                _txtWinResult.text = "WIN !";
                _txtPlayerWinResult.text = (winResult == WinResultEnum.Player1 ? "Player 1" : "Player 2");
            }
            _anmtrGameResult.SetTrigger("Show");
            _anmtrPlayerTurn.gameObject.SetActive(false);
            ResetValues(GameState.player1WinCount, GameState.player2WinCount);

        }
        public void ResetValues(int player1Count, int player2Count)
        {
            _txtPlayer1.text = player1Count.ToString();
            _txtPlayer2.text = player2Count.ToString();
        }
        private void ClearSessionData()
        {
            GameState.player1WinCount = 0;
            GameState.player2WinCount = 0;
        }
    }
}