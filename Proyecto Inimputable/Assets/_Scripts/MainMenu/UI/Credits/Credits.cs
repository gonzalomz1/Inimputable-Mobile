using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] private Animation animationComponent;
    [SerializeField] private AnimationClip animClipLogo;
    [SerializeField] private SplashScreen splashScreen;

    public void PlayLogoAnimation()
    {
        if (animationComponent == null || animClipLogo == null) return;
        animationComponent.clip = animClipLogo;
        animationComponent.Play();
    }
    
    public void AnimationFinished()
    {
        splashScreen.OnCreditsAnimationFinished();
    }

}
