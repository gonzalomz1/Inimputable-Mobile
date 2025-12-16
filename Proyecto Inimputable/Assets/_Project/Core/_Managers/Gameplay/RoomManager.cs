using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [Header("Room Doors")]
    public Door FR_Entrance; // Door coming from the Basement
    public Door FR_Exit;     // Door going to the Second Room
    public Door SR_Entrance; // Entrance to Second Room (if needed)

    // State Machine Definition
    public enum RoomState
    {
        BasementZone,    // We are starting in the basement.
        FirstRoomLocked, // We entered the First Room, doors are locked behind us.
        FirstRoomCleared, // We killed the thief, exit is open.
        SecondRoomLocked // We entered the Second Room, doors are locked behind us.
    }

    [Header("Current State")]
    public RoomState currentState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Subscribe to Game Manager to know when the gameplay actually starts
        if (GameManager.instance != null)
        {
            GameManager.instance.GameplayStart += OnGameplayStart;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.GameplayStart -= OnGameplayStart;
        }
        
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.ObjectiveCompleted -= OnObjectiveCompleted;
        }
    }

    private void OnGameplayStart()
    {
        // Now we really start the logic
        Debug.Log("RoomManager: Gameplay Started. Initializing.");

        // Subscribe to the Objective Manager
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.ObjectiveCompleted += OnObjectiveCompleted;
        }

        // Initialize state
        SetState(RoomState.BasementZone);
    }



    [Header("Objective References")]
    [Tooltip("Reference to the objective that triggers locking the room (Exit Basement)")]
    public ObjectiveBase objectiveExitBasement;
    [Tooltip("Reference to the objective that triggers unlocking the room (Kill Thief)")]
    public ObjectiveBase objectiveKillThief;

    // This function decides what to do when an objective is finished
    private void OnObjectiveCompleted(string completedId)
    {
        Debug.Log($"RoomManager: Objective Completed with ID: {completedId}");

        if (objectiveExitBasement != null && completedId == objectiveExitBasement.Id)
        {
            SetState(RoomState.FirstRoomLocked);
        }
        else if (objectiveKillThief != null && completedId == objectiveKillThief.Id)
        {
            SetState(RoomState.FirstRoomCleared);
        }
        else
        
        {
            Debug.LogWarning($"RoomManager: Unhandled Objective ID: {completedId}");
        }
    }

    // This is the Brain of the Room Manager. It applies the rules for each state.
    public void SetState(RoomState newState)
    {
        currentState = newState;
        Debug.Log($"RoomManager: Switching State to {newState}");

        switch (currentState)
        {
            case RoomState.BasementZone:
                // Start of game. We assume default state or specific locks if needed.
                // For now, we don't force locks here unless we want to lock the player IN the basement until something happens.
                break;

            case RoomState.FirstRoomLocked:
                // The player has entered the First Room.
                // Rule 1: Trapped! Lock the door behind (Entrance).
                Debug.Log($"RoomManager: Locking {FR_Entrance.name} (Entrance)");
                FR_Entrance.LockStateDoor(); 
                // Rule 2: No Escape! Lock the door ahead (Exit).
                Debug.Log($"RoomManager: Locking {FR_Exit.name} (Exit)");
                FR_Exit.LockStateDoor();
                break;

            case RoomState.FirstRoomCleared:
                // The player killed the enemy.
                // Rule: Freedom! Unlock the exit door.
                Debug.Log($"RoomManager: Unlocking {FR_Exit.name} (Exit) - Room Cleared");
                FR_Exit.DefaultStateDoor();
                break;
        }
    }
}
