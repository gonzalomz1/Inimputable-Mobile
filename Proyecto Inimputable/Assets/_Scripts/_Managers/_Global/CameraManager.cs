using UnityEngine;

public enum CameraType
{
    Menu,
    Cinematic,
    Gameplay
}
public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [Header("Cámaras")]
    public Camera menuCamera;
    public Camera cinematicCamera;
    public Camera gameplayCamera;

    [Header("Animator de Transiciones")]
    public Animator cinematicAnimator; // para animar cámaras como la de resaca

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(gameObject);
    }

    public void SwitchToMenuCamera()
    {
        ActivateOnly(menuCamera);
    }

    public void SwitchToGameplayCamera()
    {
        ActivateOnly(gameplayCamera);
    }

    public void SwitchToCinematicCamera(string animationTrigger)
    {
        ActivateOnly(cinematicCamera);
        cinematicAnimator.SetTrigger(animationTrigger);
    }

    private void ActivateOnly(Camera targetCamera)
    {
        if (menuCamera != null)menuCamera.gameObject.SetActive(false);
        if (cinematicCamera != null)cinematicCamera.gameObject.SetActive(false);
        if (gameplayCamera != null)gameplayCamera.gameObject.SetActive(false);

        targetCamera.gameObject.SetActive(true);
    }
}