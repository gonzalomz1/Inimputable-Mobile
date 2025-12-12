using UnityEngine;
using System;
using JetBrains.Annotations;

public abstract class ItemBase : MonoBehaviour, IItem
{
    [Header("Item Data")]
    [SerializeField] private string _id;
    [SerializeField, TextArea] private string _description;
    public string Id => _id;
    public string Description => _description;
    
    public event Action OnPickup;
    public event Action OnUse;

    public void TriggerCallOnPickUp()
    {
        OnPickup?.Invoke();
    }
}