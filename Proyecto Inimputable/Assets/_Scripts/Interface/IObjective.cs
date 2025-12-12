// IObjective.cs
using System;
using UnityEngine;

public interface IObjective
{
    string Id {get;}
    string Description {get;}
    bool IsCompleted {get;}
    event Action<IObjective> OnStarted;
    event Action<IObjective> OnCompleted;
    void Begin();
    void Abort();
    void Tick(); // call from manager if needed.
}