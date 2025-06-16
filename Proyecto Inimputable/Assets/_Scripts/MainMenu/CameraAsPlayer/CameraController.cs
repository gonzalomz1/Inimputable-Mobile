using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CameraView cameraView;

    public void SetTvClipAngle()
    {
        cameraView.SetTvClipAngle();
    }
    public void SetMainMenuAngle()
    {
        cameraView.SetMainMenuAngle();
    }
    public void SetCreditsAngle()
    {
    }
    public void ReturnToMainMenuAngle()
    {
    }

}
