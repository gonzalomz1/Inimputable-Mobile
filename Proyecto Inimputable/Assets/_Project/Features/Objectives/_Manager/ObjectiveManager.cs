using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{    
    public static ObjectiveManager Instance {get; private set;}
    [SerializeField] private List <ObjectiveBase> _objectives;
    private int _currentIndex = -1;
    private bool _gameplayActive = false;
    private ObjectiveBase Current =>
    (_currentIndex < _objectives.Count) 
    ? _objectives[_currentIndex] 
    : null;

    public event Action SpawnEnemy;
    public event Action<string> ObjectiveUpdate;
    public event Action<string> ObjectiveCompleted;

    [SerializeField] private Transform firstRoomEnemySpawnPoint;
    public Transform room1SpawnPoint; // Asignar en Inspector (Entrada Sala 1)
    public Transform room2SpawnPoint; // Asignar en Inspector (Entrada Sala 2)
    public Transform pistolSpawnPoint; // Asignar en Inspector

    private void Awake()
    {
        if (Instance != null & Instance != this) { Destroy(gameObject); return;}
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SpawnEnemy += OnSpawnEnemyRequest;
    }
    
    // ... existing Enable/Disable ...

    private void OnSpawnEnemyRequest()
    {
        if (firstRoomEnemySpawnPoint == null)
        {
            Debug.LogError("ObjectiveManager: No enemySpawnPoint assigned!");
            return;
        }

        // TESTING: Spawning a MELEE enemy instead of default (Ranged)
        GameObject spawnedEnemy = Spawner.instance.SpawnEnemy(firstRoomEnemySpawnPoint, TurroController.EnemyType.Melee);
        
        if (spawnedEnemy != null && Current is KillThiefObjective killObjective)
        {
            killObjective.SetTarget(spawnedEnemy);
        }
    }

    private void OnEnable()
    {
        GameManager.instance.GameplayStart += OnGameplayStart;
        GameManager.instance.GameplayPause += OnGameplayPause;
        GameManager.instance.GameplayResume += OnGameplayResume;
        GameManager.instance.GameplayExit += OnGameplayExit;
    }

    private void OnDisable()
    {
        GameManager.instance.GameplayStart -= OnGameplayStart;
        GameManager.instance.GameplayPause -= OnGameplayPause;
        GameManager.instance.GameplayResume -= OnGameplayResume;
        GameManager.instance.GameplayExit -= OnGameplayExit;
    }

    private void Update()
    {
        if (!_gameplayActive || Current == null) return;

        // el manager no tiene logica -> reenvia tick a los objetivos
        Current.Tick();
    }

    private void OnGameplayStart()
    {
        _gameplayActive = true;
        ResetAllObjectives(); // Ensure fresh state for new game
        if (Spawner.instance != null && pistolSpawnPoint != null) Spawner.instance.SpawnPistol(pistolSpawnPoint);
        StartNextObjective();
    }

    private void OnGameplayPause()
    {
        _gameplayActive = false;
    }

    private void OnGameplayResume()
    {
        _gameplayActive = true;
    }

    private void OnGameplayExit()
    {
        // cleanup UI if needed
        Current?.Abort(); 
        
        _gameplayActive = false;
        _currentIndex = -1;
    }

    private void StartNextObjective()
    {
        _currentIndex++;
        if (Current == null)
        {
            Debug.Log("All objectives completed!");
            
            // Check if we finished the last objective (assuming success flow)
            if (_currentIndex >= _objectives.Count && GameManager.instance != null)
            {
                GameManager.instance.SetWinGame();
            }
            
            ObjectiveUpdate?.Invoke("Mision Cumplida");
            return;
        }

        // --- SPAWN LOGIC ---
        // Index 2: Kill Thief - Needs enemy spawn immediately!
        // Index 4: Survive - Configured via internal DifficultyManager logic, but if needed spawn here.
        
        if (_currentIndex == 2)
        {
             SpawnEnemy?.Invoke();
        }

        Current.OnCompleted += OnObjectiveCompleted;
        Current.Begin();
        ObjectiveUpdate?.Invoke(Current.Description);
    }



    private void OnObjectiveCompleted(IObjective obj)
    {
        Debug.Log("Current objective completed!");
        ObjectiveCompleted?.Invoke(obj.Id); // Notify listeners with the ID string
        obj.OnCompleted -= OnObjectiveCompleted;
        StartNextObjective();
    }

    public void ResetCurrentObjective()
    {
        // Handle post-game retry: rewind to the last objective
        if (_currentIndex >= _objectives.Count && _objectives.Count > 0)
        {
             _currentIndex = _objectives.Count - 1;
        }

        // 1. Reset Difficulty Time
        if (SurvivalDifficultyManager.Instance != null)
            SurvivalDifficultyManager.Instance.ResetDifficulty();

        // 2. Clear Enemies
        if (Spawner.instance != null)
            Spawner.instance.ClearAllActiveEnemies();

        // Respawn Pistol (needs to be after ClearAllActiveEnemies which clears pool)
        if (Spawner.instance != null)
        {
            // If checking index >= 2, the player should visually HAVE the pistol.
            // Instead of spawning a pickup, we force equip it.
            if (_currentIndex >= 2)
            {
                 if (WeaponController.instance != null)
                 {
                     WeaponController.instance.PickUpWeapon(WeaponType.Pistol);
                 }
            }
            else if (pistolSpawnPoint != null)
            {
                // If earlier stage, spawn the pickup item normally
                Spawner.instance.SpawnPistol(pistolSpawnPoint);
            }
        } 

        // 3. Reset specific objective state if needed
        // ... call Abort/Begin
        
        // 4. Force Update UI Text
        if (Current != null) ObjectiveUpdate?.Invoke(Current.Description);

        // 3. Reset specific objective state if needed
        if (Current != null)
        {
            Current.Abort(); 
            
            // Re-subscribe to ensure completion triggers next step/win state
            Current.OnCompleted -= OnObjectiveCompleted;
            Current.OnCompleted += OnObjectiveCompleted;
            
            Current.Begin(); 
            
            // 4. Force Spawn if needed upon RESET
            if (_currentIndex == 2)
            {
                SpawnEnemy?.Invoke();
            }
        }
    }

    public Transform playerStartPoint; // Asignar en Inspector (Punto de inicio del juego)

    // ... (existing code)

    public Transform GetRespawnPoint()
    {
        // Index 2: Kill Thief -> Entrar Sala 1
        // Index 4: Survive -> Entrar Sala 2
        
        if (_currentIndex == 2) 
        {
             return room1SpawnPoint; 
        }
        else if (_currentIndex == 4)
        {
             return room2SpawnPoint;
        }
        else
        {
             // Default / Safety / Start
             return playerStartPoint; 
        }
    }

    private void ResetAllObjectives()
    {
        _currentIndex = -1;
        foreach (var obj in _objectives)
        {
            if (obj != null) obj.ResetObjective();
        }
    }
}