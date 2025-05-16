using UnityEngine;
using System.Linq;

public class AimAssist : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float maxDistance = 25f;             // Max distance of enemy to consider
    [SerializeField] private float screenRadius = 100f;           // Radius in pixels from the center where help activates
    [SerializeField][Range(0f, 1f)] private float aimAssistStrength = 0.3f; // Strength of the help

    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerData playerData;

    private Transform currentTarget;

    void Awake()
    {
        mainCamera = Camera.main;
        playerData = GameObject.FindWithTag("Player").GetComponent<PlayerData>();
    }
/*
    void Update()
    {
        if (currentTarget == null)
        {
            playerData.isAimAssistActive = false;
            return;
        }
        FindBestTarget();
        ApplyAimAssist();
    }
    */


    void FindBestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemies");

        currentTarget = null;
        float closestDistance = float.MaxValue;

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            Vector3 screenPos = mainCamera.WorldToScreenPoint(enemy.transform.position);

            if (screenPos.z < 0) continue; // Está detrás de la cámara

            float dist = Vector2.Distance(screenPos, screenCenter);

            if (dist < screenRadius && dist < closestDistance)
            {
                float worldDist = Vector3.Distance(mainCamera.transform.position, enemy.transform.position);
                if (worldDist < maxDistance)
                {
                    closestDistance = dist;
                    currentTarget = enemy.transform;
                }
            }
        }
    }

    void ApplyAimAssist()
    {
        if (currentTarget == null) return;
        if (!IsValidTarget(currentTarget)) return;
        playerData.isAimAssistActive = true;
        Vector3 targetDir = currentTarget.position - mainCamera.transform.position;
        targetDir.Normalize();
        Quaternion currentRotation = mainCamera.transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(targetDir);

        Quaternion assistedRotation = Quaternion.Slerp(currentRotation, targetRotation, aimAssistStrength * Time.deltaTime * 60f);
        mainCamera.transform.rotation = assistedRotation;

    }

    public void SetAimAssistStrength(float value)
    {
        aimAssistStrength = Mathf.Clamp01(value);
    }



    bool IsValidTarget(Transform enemy)
    {
        var turro = enemy.GetComponent<TurroController>();
        return turro != null && (turro.turroState != EnemyState.Dead || turro.turroState != EnemyState.Die);
    }
}