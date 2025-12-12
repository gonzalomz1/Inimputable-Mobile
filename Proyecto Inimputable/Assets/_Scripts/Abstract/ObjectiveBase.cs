using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectiveBase : MonoBehaviour, IObjective
{
    [Header("Objective Data")]
    [SerializeField] private string _id;
    [SerializeField, TextArea] private string _description;

    public string Id => _id;
    public string Description => _description;
    public bool IsCompleted { get; protected set; }

    public event Action<IObjective> OnStarted;
    public event Action<IObjective> OnCompleted;

    [Header("Conditions")]
    [SerializeField] private List<ObjectiveCondition> _conditions = new();
    public List<ObjectiveCondition> GetConditions() => _conditions;

    protected virtual void Awake()
    {
        if (string.IsNullOrEmpty(_id))
            _id = Guid.NewGuid().ToString();

        // Desactivar condiciones hasta que se active el objetivo
        foreach (var c in _conditions)
            c.gameObject.SetActive(false);
    }

    public virtual void Begin()
    {
        IsCompleted = false;

        foreach (var cond in _conditions)
        {
            if (cond == null) continue; // Guard against empty slots in Inspector
            cond.ResetCondition();         // ← recomendado en runtime
            cond.gameObject.SetActive(true);
            cond.Initialize(this);
        }

        OnStarted?.Invoke(this);
    }

    public void NotifyConditionSatisfied()
    {
        CheckForCompletion();
    }

    protected void CheckForCompletion()
    {
        if (IsCompleted) return;

        foreach (var cond in _conditions)
        {
            if (!cond.IsSatisfied)
                return;
        }

        Complete();
    }

    protected virtual void Complete()
    {
        IsCompleted = true;

        foreach (var cond in _conditions)
        {
            cond.Cleanup();
            cond.gameObject.SetActive(false);
        }
        OnCompleted?.Invoke(this);
    }

    public virtual void Abort()
    {
        foreach (var cond in _conditions)
        {
            cond.Cleanup();
            cond.gameObject.SetActive(false);
        }
    }

    public virtual void ResetObjective()
    {
        IsCompleted = false;
        foreach (var cond in _conditions)
        {
            cond.ResetCondition();
            cond.gameObject.SetActive(false);
        }
    }

    public abstract void Tick();
}