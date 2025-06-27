using UnityEngine;

public class CameraView : MonoBehaviour
{
    [SerializeField] private Animator animator;

    void Awake()
    {
        if (animator) animator = GetComponent<Animator>();
    }

    public void SetTvClipAngle()
    {
        animator.SetTrigger("InFrontOfTv");
    }
    public void SetMainMenuAngle()
    {
        animator.SetTrigger("MainMenu");
    }
    public void SetCreditsAngle()
    {
        animator.SetTrigger("Credits");
    }

    public void FromCreditsToMainMenu()
    {
        animator.SetTrigger("CreditsToMainMenu");
    }
}
