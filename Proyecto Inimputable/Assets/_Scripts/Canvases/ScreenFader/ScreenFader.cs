using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{

    [SerializeField] private Animator animator;

    public void FadeIn()
    {
        if (!this.gameObject.activeInHierarchy) this.gameObject.SetActive(true);
        animator.SetTrigger("FadeIn");
    }

    public void FadeOut()
    {
        if (!this.gameObject.activeInHierarchy) this.gameObject.SetActive(true);
        animator.SetTrigger("FadeOut");
    }
}
