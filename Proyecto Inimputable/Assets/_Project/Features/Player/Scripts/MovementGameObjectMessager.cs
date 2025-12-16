using System;
using UnityEngine;

public class MovementGameObjectMessager : MonoBehaviour
{
    public event Action MovementSoundRequest;

    public void MovementSound()
    {
        MovementSoundRequest?.Invoke();
    }
}
