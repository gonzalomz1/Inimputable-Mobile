using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerStats : MonoBehaviour 
{
    public Slider healthBar;
    public Slider inimputabilityBar;
    public Slider manaBar;

    public Gradient healthGradient;
    public Image healthFill;
    public SliderBarShake healthBarShake;

    void Awake()
    {
        GameManager.instance.GameplayStart += OnGameplayStart;
        
        // Hide Player Health Background
        Transform bg = healthBar.transform.Find("Background");
        if (bg != null && bg.TryGetComponent(out Image bgImage))
        {
            bgImage.color = Color.clear;
        }
    }

    void OnGameplayStart()
    {
        GameObject player = GameObject.FindWithTag("Player");
        PlayerData playerData = player.GetComponent<PlayerData>();
        SetBarMaxValue(healthBar, playerData.maxHealth);
        SetBarCurrentValue(healthBar, playerData.currentHealth);
    }

    public void FirstExamMode()
    {
        healthBar.gameObject.SetActive(true);
        inimputabilityBar.gameObject.SetActive(false);
        manaBar.gameObject.SetActive(false);
    }

    public void SetBarMaxValue(Slider bar, int maxValue)
    {
        bar.maxValue = maxValue;
        if (bar == healthBar) 
        {
            healthFill.color = healthGradient.Evaluate(1f);
        }
    }

    public void SetBarCurrentValue(Slider bar, int value)
    {
        bar.value = value;
        if (bar == healthBar) 
        {
            healthFill.color = healthGradient.Evaluate(healthBar.normalizedValue);
            if (healthBarShake != null) healthBarShake.UpdateHealth();
        }
    }

}