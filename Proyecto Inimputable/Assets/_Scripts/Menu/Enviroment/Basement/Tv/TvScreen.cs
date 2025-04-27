using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Video;

public class TvScreen : MonoBehaviour
{

    public VideoPlayer videoPlayer;

    public Tv tv;

    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }
    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("¡El video ha terminado!");
        
    tv.OnVideoEnd();

    }
    public void PlayVideo()
    {
        Debug.Log("REPRODUCIENDO VIDEO");
        videoPlayer.Play();

    }
 
}
