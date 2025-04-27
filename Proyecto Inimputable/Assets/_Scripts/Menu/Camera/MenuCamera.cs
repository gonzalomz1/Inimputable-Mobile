using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
  public AnimationClip mainMenuClip;
  public Animation animationComponent;

  void Awake()
  {
    if (animationComponent == null) animationComponent = GetComponent<Animation>();
    animationComponent.enabled = false;
    animationComponent.clip = null;
    
  }

  public void MainMenuAngle()
  {
    PlayAnimation(mainMenuClip);
  }
  private void PlayAnimation(AnimationClip clip)
  {
      animationComponent.clip = clip;
      animationComponent.enabled = true;
      animationComponent.Play();
  }
}



