using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class MenuCamera : MonoBehaviour
{
  public AnimationClip mainMenuClip;
  public AnimationClip creditsClip;

  public AnimationClip creditsReturnClip;
  public Animation animationComponent;
  public MenuManager menuManager;
  
  public MenuScreen menuScreen;
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
  public void CreditsAngle()
  {
     PlayAnimation(creditsClip);
  }
  public void CreditsReturnAngle(){
      PlayAnimation(creditsReturnClip);
    
  }
  private void PlayAnimation(AnimationClip clip)
  {
      animationComponent.clip = clip;
      animationComponent.enabled = true;
      animationComponent.Play();
  }
public void OnAnimationFinished(string nameOfClip){
  if (nameOfClip == "MainMenu"){
    menuManager.SetMainMenuTexts();
  }
  if(nameOfClip == "Credits"){
   menuScreen.CreditsArrow(true);

  }
  if(nameOfClip == "CreditsReturn"){
  menuScreen.MainMenuButtons(true);
  }
  
}
 public void CreditsClip()
  {
    PlayAnimation(creditsClip);
  }
  

}











