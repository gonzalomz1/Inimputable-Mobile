using UnityEngine;

public class TurroView : MonoBehaviour
{
    public Animator animator;

    public SpriteRenderer spriteRenderer;

    void Awake()
    {
        if (spriteRenderer != null) HideSprite();
    }
    public void HideSprite()
    {
        spriteRenderer.enabled = false;
    }

    public void ShowSprite()
    {
        spriteRenderer.enabled = true;
    }
}
