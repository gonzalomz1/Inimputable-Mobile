using UnityEngine;

public class TvScreen : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Tv tv;
    public AudioSource audioSource;
    void OnVideoEnd()
    {
        Debug.Log("¡El video ha terminado!");
        tv.OnVideoEnd();
    }
    public void PlayVideo()
    {
        Debug.Log("REPRODUCIENDO VIDEO");
        animator.SetTrigger("Clip");
    }

    public void PlayAudioInimputable()
    {
        audioSource.Play();
    }
}
