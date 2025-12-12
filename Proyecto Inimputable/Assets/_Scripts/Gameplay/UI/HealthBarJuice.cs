using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarJuice : MonoBehaviour
{
    [Header("Shake Settings")]
    public Transform shakeTarget; // The object to shake (usually the bar container)
    public float shakeMagnitude = 0.2f; // Reduced default for World Space
    public float shakeDuration = 0.2f;

    private Coroutine shakeCoroutine;
    private Vector3 originalShakePos;

    void Awake()
    {
        if (shakeTarget == null) shakeTarget = transform;
        originalShakePos = shakeTarget.localPosition;
    }

    public void RefreshOriginalPosition()
    {
         if (shakeTarget == null) shakeTarget = transform;
         originalShakePos = shakeTarget.localPosition;
    }

    public void UpdateHealth()
    {
        // 1. Trigger Shake
        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            // Horizontal Only Shake (X axis)
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            
            shakeTarget.localPosition = originalShakePos + new Vector3(x, 0, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }
        shakeTarget.localPosition = originalShakePos;
    }
}
