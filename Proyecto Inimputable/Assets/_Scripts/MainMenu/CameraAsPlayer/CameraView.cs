using UnityEngine;

public class CameraView : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Awake()
    {
        if (animator) animator = GetComponent<Animator>();
        if (spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
