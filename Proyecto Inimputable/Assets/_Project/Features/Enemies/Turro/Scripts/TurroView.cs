using System.Collections;
using UnityEngine;

public class TurroView : MonoBehaviour
{
    public Animator animator;

    public SpriteRenderer spriteRenderer;

    public Color damageColor = Color.red;
    public float flashDuration = 0.2f;

    public AudioSource takeDamageAudioSource;

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

    private MaterialPropertyBlock _propBlock;

    public void SetColor(Color color)
    {
        if (spriteRenderer != null)
        {
            if (_propBlock == null) _propBlock = new MaterialPropertyBlock();
            
            spriteRenderer.GetPropertyBlock(_propBlock);
            _propBlock.SetColor("_BaseColor", color); // URP "Base Map" color property
            spriteRenderer.SetPropertyBlock(_propBlock);

            // debug
            Debug.Log($"[TurroView] MaterialPropertyBlock set _BaseColor to {color}");
        }
        else
        {
             Debug.LogError("[TurroView] SpriteRenderer is NULL!");
        }
    }

    public void FlashDamageColor()
    {
        StartCoroutine(DamageFlashRoutine());
    }

    public void TakeDamageSound()
    {
        takeDamageAudioSource.Play();
    }

    private IEnumerator DamageFlashRoutine()
    {
        spriteRenderer.material.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.material.color = originalColor;
    }

    // Health Bar Reference
    public TurroHealthBar healthBar;

    public void InitializeHealthBar(int maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(true);
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    public void UpdateHealthBar(int currentHealth)
    {
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }

    public void ToggleHealthBar(bool isActive)
    {
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(isActive);
        }
    }

    public Animator GetAnimatorComponent()
    {
        return animator;
    }
}
