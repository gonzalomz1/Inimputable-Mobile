using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : GameplayCanvas
{
    public static UICanvas Instance { get; private set; }
    public UIPlayerStats uIPlayerStats;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public override void SetActiveCanvas(bool isActive)
    {
        gameObject.SetActive(isActive);
    }


    public void StartUIPlayerStats()
    {
        uIPlayerStats.FirstExamMode();
    }
}
