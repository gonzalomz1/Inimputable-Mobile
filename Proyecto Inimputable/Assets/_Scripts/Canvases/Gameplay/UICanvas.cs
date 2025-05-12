using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : GameplayCanvas
{
    public UIPlayerStats uIPlayerStats;
    public override void SetActiveCanvas(bool isActive){
        gameObject.SetActive(isActive);
    }

    public void StartUIPlayerStats()
    {
        uIPlayerStats.FirstExamMode();
    }
}
