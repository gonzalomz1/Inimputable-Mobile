using UnityEngine;
using UnityEngine.UI;

public class TurroHealthBar : MonoBehaviour
{
    public Slider slider;
    public Camera playerCamera;
    public SliderBarShake healthBarShake;

    public Vector3 positionOffset = new Vector3(0, 2.5f, 0);

    void Start()
    {
        playerCamera = CameraManager.instance.GetPlayerGameplayCamera();
        transform.localPosition = positionOffset;
        if (healthBarShake != null) healthBarShake.RefreshOriginalPosition();

        // Hide Background
        Transform bg = slider.transform.Find("Background");
        if (bg != null && bg.TryGetComponent(out Image bgImage))
        {
            bgImage.color = Color.clear;
        }
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        if (healthBarShake != null) healthBarShake.UpdateHealth();
    }

    void LateUpdate()
    {
        // Billboard effect: Always face the camera
        if (playerCamera != null)
        {
            transform.LookAt(transform.position + playerCamera.transform.rotation * Vector3.forward,
                             playerCamera.transform.rotation * Vector3.up);
        }
        else
        {
            Debug.LogWarning("TurroHealthBar: PlayerCamera is null.");
            playerCamera = CameraManager.instance.GetPlayerGameplayCamera();
            return;
        }
    }
}
