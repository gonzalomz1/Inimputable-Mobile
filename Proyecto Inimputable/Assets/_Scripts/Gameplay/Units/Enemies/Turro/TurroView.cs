using System.Collections;
using UnityEngine;

public class TurroView : MonoBehaviour
{
    public Animator animator;

    public SpriteRenderer spriteRenderer;

    public Color damageColor = Color.red;
    public float flashDuration = 0.2f;

    private Color originalColor;

    void Awake()
    {
        if (spriteRenderer != null) HideSprite();
        originalColor = spriteRenderer.material.color;
    }

    public void HideSprite()
    {
        spriteRenderer.enabled = false;
    }

    public void ShowSprite()
    {
        spriteRenderer.enabled = true;
    }

    public void FlashDamageColor()
    {
        StartCoroutine(DamageFlashRoutine());
    }

    private IEnumerator DamageFlashRoutine()
    {
        spriteRenderer.material.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.material.color = originalColor;
    }
}
