using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : GameplayCanvas
{
    public override void SetActiveCanvas(bool isActive){
        gameObject.SetActive(isActive);
    }
}
