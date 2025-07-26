using Unity.VisualScripting;
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
    public Animator cinematicAnimator; // to animate cameras like hang over (gameplay start)

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(gameObject);
        SubscribeToGameManagerEvents();
    }

    private void SubscribeToGameManagerEvents()
    {
        GameManager.instance.GameplayStart += OnGameplayStart;
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
        if (menuCamera != null) menuCamera.gameObject.SetActive(false);
        if (cinematicCamera != null) cinematicCamera.gameObject.SetActive(false);
        if (gameplayCamera != null) gameplayCamera.gameObject.SetActive(false);
        //NOTE: gameplay camara is actually the player's camera. The game object itself its managed by PlayerManager with the same event call from GameManager.
        targetCamera.gameObject.SetActive(true);
    }

    private void OnGameplayStart()
    {
        SwitchToGameplayCamera();
    }
}