using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public Animation animationComponent;
    public AnimationClip animClipLogo;
    public MenuManager menuManager;

    
    void OnEnable()
    {
        if (animationComponent == null)
        {
            return;
        }
        animationComponent.clip = animClipLogo;
        animationComponent.Play();
    }

    public void AnimationFinished (){
    menuManager.OnCreditsAnimationFinished();


    }



}
