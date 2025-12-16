using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICanvas : GameplayCanvas
{
    public static UICanvas Instance { get; private set; }
    public UIPlayerStats uIPlayerStats;
    public TextMeshProUGUI uIObjectiveText;
    public TextMeshProUGUI uIObjectiveTimer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        if(uIObjectiveTimer != null) 
            uIObjectiveTimer.gameObject.SetActive(false);
    }

    private void Start()
    {
        SubscribeToGameManagerEvents();
        SubscribeToObjectiveManagerEvents();
    }

    void SubscribeToGameManagerEvents()
    {
        GameManager.instance.GameplayStart += OnGameplayStart;
    }

    void SubscribeToObjectiveManagerEvents()
    {
        ObjectiveManager.Instance.ObjectiveUpdate += UpdateObjectiveText;
    }

    private void UpdateObjectiveText(string text)
    {
        uIObjectiveText.text = text;
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

    public void ToggleObjectiveTimer(bool show)
    {
        if (uIObjectiveTimer != null)
            uIObjectiveTimer.gameObject.SetActive(show);
    }

    public void UpdateObjectiveTimer(float time)
    {
        if (uIObjectiveTimer != null)
        {
            int minutes = Mathf.FloorToInt(time / 60F);
            int seconds = Mathf.FloorToInt(time % 60F);
            uIObjectiveTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

}
