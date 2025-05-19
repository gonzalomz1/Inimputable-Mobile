using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
   public static GameFlowManager Instance { get; private set; }

   [SerializeField] private CanvasManager canvasManager;
   [SerializeField] private EnemiesManager enemiesManager;
   [SerializeField] private WeaponController weaponController;

   public GameFlowState currentState;

   private void Awake()
   {
      if (Instance != null && Instance != this)
      {
         Destroy(gameObject);
         return;
      }
      Instance = this;
   }

   private void Start()
   {
      SetGameState(GameFlowState.StartGameplay); // temporal for TEST_Room
   }

   public void SetGameState(GameFlowState newState)
   {
      if (currentState == newState) return;

      currentState = newState;

      switch (newState)
      {
         case GameFlowState.Loading:
            canvasManager.DisableInput();
            break;
         case GameFlowState.StartGameplay:
            weaponController.StartWeapons();
            canvasManager.StartGameplay();
            ResumeGame();
            break;
         case GameFlowState.Paused:
            canvasManager.PauseMode();
            PauseGame();
            break;
         case GameFlowState.ResumeGameplay:
            canvasManager.ResumeGameplay();
            ResumeGame();
            break;
         case GameFlowState.Cinematic:
         case GameFlowState.GameOver:
            PauseGame();
            bool checkPlayer = IsPlayerDead();
            canvasManager.GameOver();
            if (checkPlayer)
            {
               canvasManager.menuCanvas.SetLoseState();
            }
            else
            {
               canvasManager.menuCanvas.SetWinState();
            }
            break;
      }

      Debug.Log("Game state changed to: " + newState);
   }

   public GameFlowState GetCurrentState() => currentState;

   public void GameOver()
   {
      SetGameState(GameFlowState.GameOver);
   }

   public bool IsPlayerDead()
   {
      bool health = GameObject.FindWithTag("Player").GetComponent<PlayerData>().IsPlayerDead();
      return health;
   }

   public void PauseGame()
   {
      Time.timeScale = 0f;
   }
   public void ResumeGame()
   {
      Time.timeScale = 1f;
   }
}
