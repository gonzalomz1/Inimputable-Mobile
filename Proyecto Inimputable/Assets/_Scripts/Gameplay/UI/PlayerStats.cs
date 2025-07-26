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

    void Awake()
    {
        GameManager.instance.GameplayStart += OnGameplayStart;
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
        if (bar == healthBar) healthFill.color = healthGradient.Evaluate(1f);
    }

    public void SetBarCurrentValue(Slider bar, int value)
    {
        bar.value = value;
        if (bar == healthBar) healthFill.color = healthGradient.Evaluate(healthBar.normalizedValue);
    }

}