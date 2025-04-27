using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : CustomCanvas
{
   public override void SetActiveCanvas(bool isActive)
    {
        gameObject.SetActive(isActive); // Cambia la visibilidad del canvas
    }


}
