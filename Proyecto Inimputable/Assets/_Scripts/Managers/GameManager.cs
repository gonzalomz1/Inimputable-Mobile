using UnityEngine;

public class GameManager : MonoBehaviour
{

  private UIManager uiManager;
  private AudioManager audioManager;
  private GameController gameController;


  private enum GameState { Loading, MainMenu, Playing, GameOver, Credits }
  private GameState currentState;

  void Awake()
  {
    InitializeComponents();
  }

  void Start()
  {
    LoadLoadingScreen();
  }

  void InitializeComponents()
  {
    Debug.Log("<GameManager> Initializing Components");
    uiManager = GetComponent<UIManager>();
    audioManager = GetComponent<AudioManager>();
    gameController = GetComponent<GameController>();
    Debug.Log("<GameManager> " + uiManager + " Loaded succesfully.");
    Debug.Log("<GameManager> " + audioManager + " Loaded succesfully.");
    Debug.Log("<GameManager> " + gameController + " Loaded succesfully.");
  }

  void LoadLoadingScreen()
  {

  }

}
