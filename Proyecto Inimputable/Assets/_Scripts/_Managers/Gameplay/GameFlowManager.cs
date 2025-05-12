using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
   public static GameFlowManager Instance { get; private set; }

   [SerializeField] private CanvasManager canvasManager;
   [SerializeField] private EnemiesManager enemiesManager;

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
      SetGameState(GameFlowState.Gameplay); // temporal for TEST_Room
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
         case GameFlowState.Gameplay:
            canvasManager.StartGameplay();
            break;
         case GameFlowState.Paused:
         case GameFlowState.Cinematic:
         case GameFlowState.GameOver:
            canvasManager.DisableInput();
            break;
      }

      Debug.Log("Game state changed to: " + newState);
   }

   public GameFlowState GetCurrentState() => currentState;

   public void GameOver()
   {
      //SetGameState(GameFlowState.GameOver);
   }
}
