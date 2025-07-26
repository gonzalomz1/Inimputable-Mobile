using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : GameplayCanvas
{
    [SerializeField] GameManager gameManager;
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
        SubscribeToGameManagerEvents();
    }

    void SubscribeToGameManagerEvents()
    {
        gameManager.GameplayStart += OnGameplayStart;
    }

    void OnGameplayStart()
    {
        StartUIPlayerStats();
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
