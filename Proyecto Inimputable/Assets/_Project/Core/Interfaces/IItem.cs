using UnityEngine;
using System;

public interface IItem
{
    string Id {get;}
    string Description {get;}

    event Action OnPickup;
    event Action OnUse;
}