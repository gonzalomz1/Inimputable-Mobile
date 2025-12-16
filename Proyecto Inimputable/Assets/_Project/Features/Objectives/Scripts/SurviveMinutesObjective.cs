using UnityEngine;

public class SurviveMinutesObjective : ObjectiveBase
{
    [Header("Spawning Settings")]
    public Transform[] spawnPoints;
    public float baseSpawnInterval = 4f;
    private float _nextSpawnTime;
    
    // Tracking active enemies
    private int _activeEnemyCount = 0;

    public override void Tick()
    {
        if (IsCompleted) return;

        float timeDelta = Time.deltaTime;
        float currentTime = 0f;

        // 1. Update Global Time
        if (SurvivalDifficultyManager.Instance != null)
        {
            currentTime = SurvivalDifficultyManager.Instance.CurrentTimeInSeconds + timeDelta;
            SurvivalDifficultyManager.Instance.UpdateDifficulty(currentTime);
            
            // Update UI Timer
            if (UICanvas.Instance != null)
            {
                UICanvas.Instance.UpdateObjectiveTimer(currentTime);
            }
        }

        // 2. Calculate Max Allowed Enemies based on Phase
        int maxEnemies = GetMaxAllowedEnemies(currentTime);

        // 3. Handle Spawning if cap not reached (or if infinite phase)
        if (maxEnemies == -1 || _activeEnemyCount < maxEnemies)
        {
            if (Time.time >= _nextSpawnTime)
            {
                SpawnWave(currentTime);
                UpdateNextSpawnTime();
            }
        }
    }

    private int GetMaxAllowedEnemies(float time)
    {
        // 6+ minutes: Unlimited
        if (time > 360f) return -1;

        // Linear progression: 
        // 0-1 min (0.X) -> 1
        // 1-2 min (1.X) -> 2
        // 2-3 min (2.X) -> 3
        // y asi...
        return Mathf.FloorToInt(time / 60f) + 1;
    }

    private void SpawnWave(float time)
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return;

        // Choose random point
        int index = UnityEngine.Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];

        // Safety Check
        if (spawnPoint == null)
        {
            Debug.LogWarning("SurviveMinutesObjective: Spawn Point missing.");
            return;
        }
        

        // 50/50 Ranged/Melee choose
        TurroController.EnemyType typeToSpawn = 
        (UnityEngine.Random.value > 0.5f) ? TurroController.EnemyType.Melee : TurroController.EnemyType.Ranged;

        GameObject enemyObj = Spawner.instance.SpawnEnemy(spawnPoint, typeToSpawn);
        
        if (enemyObj != null)
        {
            _activeEnemyCount++;
            TurroController controller = enemyObj.GetComponent<TurroController>();
            if (controller != null)
            {
                // Unsubscribe first to avoid double subscription if pooled (though Reset usually handles this, safe practice)
                controller.Dead -= OnEnemyDeath;
                controller.Dead += OnEnemyDeath;
            }
        }
    }

    private void OnEnemyDeath()
    {
        _activeEnemyCount--;
        if (_activeEnemyCount < 0) _activeEnemyCount = 0;
    }

    private void UpdateNextSpawnTime()
    {
        float multiplier = (SurvivalDifficultyManager.Instance != null) ? SurvivalDifficultyManager.Instance.SpawnRateMultiplier : 1f;
        // Avoid division by zero
        if (multiplier <= 0.1f) multiplier = 0.1f;
        
        float currentInterval = baseSpawnInterval / multiplier;
        _nextSpawnTime = Time.time + currentInterval;
    }

    public override void Begin()
    {
        // RESET SPAWN STATE
        _activeEnemyCount = 0; 
        _nextSpawnTime = Time.time + 1f; // Initial small delay so it doesn't spawn instantly on frame 0

        base.Begin();
        if (UICanvas.Instance != null)
            UICanvas.Instance.ToggleObjectiveTimer(true);

        // Sync Total Time from TimerCondition if available
        var timerCondition = GetComponentInChildren<TimerCondition>();
        if (timerCondition != null && SurvivalDifficultyManager.Instance != null)
        {
            float totalMinutes = timerCondition.TimeRequired / 60f;
            if (totalMinutes > 0)
            {
                SurvivalDifficultyManager.Instance.TotalGameMinutes = totalMinutes;
                // Debug.Log($"[SurviveMinutesObjective] Synced Total Time: {totalMinutes} mins");
            }
        }
        else
        {
             // Fallback: Check conditions list if it's not a direct child component (though unusual setup)
             foreach(var cond in GetConditions())
             {
                 if (cond is TimerCondition tCond)
                 {
                    if (SurvivalDifficultyManager.Instance != null)
                    {
                        float totalMinutes = tCond.TimeRequired / 60f;
                        SurvivalDifficultyManager.Instance.TotalGameMinutes = totalMinutes;
                        break;
                    }
                 }
             }
        }
    }

    protected override void Complete()
    {
        if (UICanvas.Instance != null)
            UICanvas.Instance.ToggleObjectiveTimer(false);
            
        base.Complete();
    }

    public override void Abort()
    {
        if (UICanvas.Instance != null)
            UICanvas.Instance.ToggleObjectiveTimer(false);

        base.Abort();
    }

    // Just to visualize in inspector
    public float CurrentTime => SurvivalDifficultyManager.Instance != null ? SurvivalDifficultyManager.Instance.CurrentTimeInSeconds : 0f;
}