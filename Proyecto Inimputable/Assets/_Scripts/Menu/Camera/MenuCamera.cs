using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class MenuCamera : MonoBehaviour
{
  public AnimationClip mainMenuClip;
  public Animation animationComponent;
  public MenuManager menuManager;

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
public void OnAnimationFinished(AnimationClip clip){
  if (clip == mainMenuClip){
  menuManager.SelectTextPosition(0);
  }


}

}



