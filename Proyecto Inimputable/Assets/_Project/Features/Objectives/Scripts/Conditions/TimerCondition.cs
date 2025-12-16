using UnityEngine;

public class TimerCondition : ObjectiveCondition
{
    [SerializeField] private float timeRequired;
    public float TimeRequired => timeRequired;
    private float timer;

    protected override void OnConditionInit()
    {
        timer = 0f;
    }

    private void Update()
    {
        if (!_initialized || IsSatisfied) return;

        timer += Time.deltaTime;
        if (timer >= timeRequired)
            MarkSatisfied();
    }
}