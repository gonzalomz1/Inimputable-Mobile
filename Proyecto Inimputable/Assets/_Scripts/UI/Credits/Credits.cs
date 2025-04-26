using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public Animation animLogo;
    public bool animFinish = false;

    
    void OnEnable()
    {
        if (animLogo == null)
        {
            Debug.LogError("No se inicializo la variable animLogo");
            return;
        }
        animLogo.Play();
        
    }

    void AnimationFinished (){
        animFinish = true;
    }


}
