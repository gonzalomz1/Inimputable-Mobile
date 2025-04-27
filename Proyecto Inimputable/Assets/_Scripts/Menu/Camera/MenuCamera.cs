using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
public AnimationClip mainMenuClip;
public Animation animationComponent;
public void MainMenuAngle()
    {
        PlayAnimation(mainMenuClip);
    }
 private void PlayAnimation(AnimationClip clip)
    {
      if (clip != null) {
     animationComponent.clip = clip;
     animationComponent.Play(); 

      }
           
    }
 
 
 }



