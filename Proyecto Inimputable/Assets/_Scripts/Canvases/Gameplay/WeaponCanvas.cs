using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCanvas : CustomCanvas
{

    public override void SetActiveCanvas(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
