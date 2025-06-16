using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : MonoBehaviour
{
    public Credits credits;

    void AnimationFinished()
    {
        credits.AnimationFinished();
    }

}
