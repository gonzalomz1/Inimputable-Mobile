using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class GunFlash : MonoBehaviour
{
    [SerializeField] Light muzzleFlashLight;
    public float flashDuration = 0.05f;

    private float flashTimer = 0f;

    void Update()
    {
        // Disparar con click izquierdo
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        // Controlar duración del flash
        if (muzzleFlashLight.enabled)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0f)
            {
                muzzleFlashLight.enabled = false;
            }
        }
        */
    }

    void Fire()
    {
        // Activar luz de flash
        muzzleFlashLight.enabled = true;
        flashTimer = flashDuration;

        // Aquí podrías insta--nciar un proyectil o raycast
        Debug.Log("Bang! Disparo realizado.");
    }
}