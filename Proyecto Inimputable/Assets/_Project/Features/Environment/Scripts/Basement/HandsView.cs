using UnityEngine;

public class HandsView : MonoBehaviour
{
    [SerializeField] private HandsController handsController;
    [SerializeField] private Animator animator;

    void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
    }

    public void SetMainMenuState()
    {
        animator.SetTrigger("MainMenu");
    }

    public void SetPlayButtonState()
    {
        animator.SetTrigger("Play");
    }

    public void OnDrinkingAnimationFinished()
    {
        handsController.ControllerDrinkAnimationFinished();
    }
}
