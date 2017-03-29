using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TicTacToe
{
    public class ControllerUiMenu : MonoBehaviour
    {
        [SerializeField]
        private string GameSceneName = "Game";

        [Header("Panel intro")]
        [SerializeField]
        private Button _btnPlay;
        [SerializeField]
        private Animator _anmtrPanelPlay;


        [Header("Panel selection")]
        [SerializeField]
        private Button _btnBoardSizeAdd;
        [SerializeField]
        private Button _btnBoardSizeSubtract;
        [SerializeField]
        private TextMeshProUGUI _txtBoardSize;

        [SerializeField]
        private Button _btnPlayer1Type;
        [SerializeField]
        private Button _btnPlayer2Type;
        [SerializeField]
        private TextMeshProUGUI _txtPlayer1Type;
        [SerializeField]
        private TextMeshProUGUI _txtPlayer2Type;
        [SerializeField]
        private Sprite _sprtCross;
        [SerializeField]
        private Sprite _sprtCircle;
        [SerializeField]
        private Button _btnHardness;
        [SerializeField]
        private TextMeshProUGUI _txtHardness;
        [SerializeField]
        private Button _btnStart;

        private PlayerType[] _playerTypes = new PlayerType[2];
        private int _boardSize = 3;
        private DificultyLevel dificultyLevel;

        void Start()
        {
            InitDefaults();
            AddListeners();
        }

        private void AddListeners()
        {
            _btnPlay.onClick.AddListener(() =>
            {
                _anmtrPanelPlay.SetTrigger("Show");
            });

            _btnPlayer1Type.onClick.AddListener(() =>
            {
                SwitchPlayerType(true);
                DisableHardnessIfNeed();
            });

            _btnPlayer2Type.onClick.AddListener(() =>
            {
                SwitchPlayerType(false);
                DisableHardnessIfNeed();
            });

            _btnBoardSizeAdd.onClick.AddListener(() =>
            {
                if (GameState.BOARD_MAX_SIZE > _boardSize)
                {
                    _boardSize++;
                    _txtBoardSize.text = "BOARD <br>" + _boardSize + " x " + _boardSize;
                    _txtBoardSize.GetComponent<Animator>().SetTrigger("PlayAdd");
                }
            });

            _btnBoardSizeSubtract.onClick.AddListener(() =>
            {
                if (GameState.BOARD_MIN_SIZE < _boardSize)
                {
                    _boardSize--;
                    _txtBoardSize.text = "BOARD <br>" + _boardSize + " x " + _boardSize;
                    _txtBoardSize.GetComponent<Animator>().SetTrigger("PlaySub");
                }
            });

            _btnHardness.onClick.AddListener(()=> {
                SwitchHadrness();
            });

            _btnStart.onClick.AddListener(() =>
            {
                SaveData();
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
            });
        }

        private void DisableHardnessIfNeed()
        {
            if(_playerTypes[0] == PlayerType.Player && _playerTypes[1] == PlayerType.Player)
            {
                _btnHardness.gameObject.SetActive(false);
            }
            else
            {
                _btnHardness.gameObject.SetActive(true);
            }
        }

        private void InitDefaults()
        {
            _boardSize = GameState.BOARD_MIN_SIZE;
            _txtBoardSize.text = "BOARD <br>" + _boardSize + " x " + _boardSize;

            _playerTypes[0] = PlayerType.CPU;
            _playerTypes[1] = PlayerType.Player;
            dificultyLevel = DificultyLevel.One;

            SwitchPlayerType(true);
            SwitchPlayerType(false);
    
            SwitchHadrness();
        }
        private void SwitchPlayerType(bool isPlayer1)
        {
            if (isPlayer1)
            {
                if (_playerTypes[0] == PlayerType.Player)
                {
                    _playerTypes[0] = PlayerType.CPU;
                    _txtPlayer1Type.text = "CPU";
                }
                else
                {
                    _playerTypes[0] = PlayerType.Player;
                    _txtPlayer1Type.text = "Brain";
                }
            }
            else
            {
                if (_playerTypes[1] == PlayerType.Player)
                {
                    _playerTypes[1] = PlayerType.CPU;
                    _txtPlayer2Type.text = "CPU";
                }
                else
                {
                    _playerTypes[1] = PlayerType.Player;
                    _txtPlayer2Type.text = "Brain";
                }
            }
        }
        private void SaveData()
        {
            GameState.playerTypes = _playerTypes;
            GameState.boardSize = _boardSize;
            GameState.dificultyLevel = dificultyLevel;
            GameState.isInitializad = true;
        }
        private void SwitchHadrness()
        {
            if(dificultyLevel == DificultyLevel.One)
            {
                _txtHardness.text = "X X";
                _txtHardness.color = new Color(0.5f, 0.5f, 0.5f);
                dificultyLevel = DificultyLevel.Two;
            }
            else if (dificultyLevel == DificultyLevel.Two)
            {
                _txtHardness.text = "X X X";
                _txtHardness.color = new Color(0, 0, 0);
                dificultyLevel = DificultyLevel.Three;
            }
            else if (dificultyLevel == DificultyLevel.Three)
            {
                _txtHardness.text = "X";
                _txtHardness.color = new Color(1f, 1f, 1f);
                dificultyLevel = DificultyLevel.One;
            }
        }
    }
}