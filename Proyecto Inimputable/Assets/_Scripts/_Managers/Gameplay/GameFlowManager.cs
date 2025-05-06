using UnityEngine;

enum GameFlowState { Loading, Active, Dead }
public class GameFlowManager : MonoBehaviour
{
   [SerializeField] private CanvasManager canvasManager;
   [SerializeField] private EnemiesManager enemiesManager;

   GameFlowState currentState;

   void Start() 
   {
      currentState = GameFlowState.Active; //hardcoreado a active para testear inputs del player
      ManageState(currentState);
   }

   void ManageState(GameFlowState state){
      switch (state)
      {
         case GameFlowState.Loading:
         break;
         case GameFlowState.Active:
            StartGameplay();
         break;
         case GameFlowState.Dead:
         break;
      }
   }

   void StartGameplay()
   {
      canvasManager.StartGameplay();
   }

}
