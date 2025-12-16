using UnityEngine;
using System;

public class SurvivalDifficultyManager : MonoBehaviour
{
    public static SurvivalDifficultyManager Instance;

    [Header("Current Difficulty")]
    public float CurrentTimeInSeconds;
    public float HealthMultiplier = 1f;
    public float DamageMultiplier = 1f;
    public float SpeedMultiplier = 1f;
    public float SpawnRateMultiplier = 1f;

    [Header("Game Configuration")]
    public float TotalGameMinutes = 8f; // Default baseline
    // on Start, SurviveMinutesObjective will set TotalGameMinutes to the value of the objective;

    public event Action<float> OnDifficultyUpdated;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public enum EnemyBuff { None, Tank, Speed, Damage }

    public void UpdateDifficulty(float time)
    {
        CurrentTimeInSeconds = time;
        OnDifficultyUpdated?.Invoke(time);
    }

    public void ResetDifficulty()
    {
        CurrentTimeInSeconds = 0f;
        HealthMultiplier = 1f;
        DamageMultiplier = 1f;
        SpeedMultiplier = 1f;
        SpawnRateMultiplier = 1f;
        OnDifficultyUpdated?.Invoke(0f);
    }

    public EnemyBuff GetRandomBuff()
    {
        // Calculate progress (0.0 to 1.0) based on TotalGameMinutes
        float totalSeconds = TotalGameMinutes * 60f;
        if (totalSeconds <= 0) totalSeconds = 480f; // Safety fallback

        float progress = CurrentTimeInSeconds / totalSeconds;
        float randomValue = UnityEngine.Random.value; 

        // Baseline (8 mins):
        // 0-1 min = 0% - 12.5% (0.125)
        // 1-3 min = 12.5% - 37.5% (0.375)
        // 3+ min  = > 37.5%

        if (progress < 0.125f) // Early Game (Equivalent to 0-1 min in 8 min game)
        {
            return EnemyBuff.None; 
        }
        else if (progress < 0.375f) // Mid Game (Equivalent to 1-3 min in 8 min game)
        {
            if (randomValue > 0.9f) return EnemyBuff.Damage;
            if (randomValue > 0.8f) return EnemyBuff.Tank;
            if (randomValue > 0.6f) return EnemyBuff.Speed;
            return EnemyBuff.None;
        }
        else // Late Game
        {
            if (randomValue > 0.8f) return EnemyBuff.Damage;
            if (randomValue > 0.6f) return EnemyBuff.Tank;
            if (randomValue > 0.3f) return EnemyBuff.Speed;
            return EnemyBuff.None;
        }
    }
}
