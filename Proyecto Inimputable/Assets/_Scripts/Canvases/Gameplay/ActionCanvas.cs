using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using System.Collections.Generic;


public class ActionCanvas : MonoBehaviour
{

    public static ActionCanvas Instance { get; private set; }
    [Header("Buttons")]
    public Button shootButton;
    public Button reloadButton;
    public Button pauseButton;
    public Button interactButton;
    private bool isShowingInteract = false;
    [SerializeField] private List<Sprite> shootSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> reloadSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> pauseSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> interactSprites = new List<Sprite>();

    [Header("Interaction Settings")]
    public float maxDistance = 5;
    public LayerMask interactableLayers;
    private IInteractive currentInteractive;
    [Header("References")]
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    public WeaponController weaponController;

    public Camera playerCamera;

    public event Action pauseRequest;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        interactButton.gameObject.SetActive(false);
    }

    public bool HandleTouch(Finger finger, out FingerRole assignedRole)
    {
        assignedRole = FingerRole.None;

        // Convert Finger.screenPosition on an event of raycast
        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.position = finger.screenPosition;

        var results = new System.Collections.Generic.List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (var result in results)
        {
            Button button = result.gameObject.GetComponent<Button>();
            if (button != null)
            {
                //Debug.Log("Botón tocado: " + button.name);
                assignedRole = FingerRole.Action;
                if (button == shootButton) button.image.sprite = shootSprites[1];
                if (button == reloadButton) button.image.sprite = reloadSprites[1];
                if (button == pauseButton) button.image.sprite = pauseSprites[1];
                if (button == interactButton) button.image.sprite = interactSprites[1];
                button.onClick.Invoke(); // Simula el click
                return true;
            }
        }
        //Debug.Log("On ActionCanvas.HandleTrouch(): returning false");
        return false;
    }

    public void HandleFingerUp(Finger finger)
    {
        if (shootButton.image.sprite == shootSprites[1]) shootButton.image.sprite = shootSprites[0];
        if (reloadButton.image.sprite == reloadSprites[1]) reloadButton.image.sprite = reloadSprites[0];
        if (pauseButton.image.sprite == pauseSprites[1]) pauseButton.image.sprite = pauseSprites[0];
        if (interactButton.image.sprite == interactSprites[1]) interactButton.image.sprite = interactSprites[0];
    }

    public void OnClickReload()
    {
        weaponController.TryReload();
    }
    public void OnClickShoot()
    {
        weaponController.TryShoot();
    }
    public void OnClickPause()
    {
        pauseRequest?.Invoke();
    }

    public void Interact()
    {
        if (currentInteractive == null) return;

        // Si el objeto es una puerta
        if (currentInteractive is Door door)
        {
            if (door.TryToOpenDoor(out Door linkedDoor))
            {
                GameManager.instance.StartCoroutine(
                    GameManager.instance.DoDoorTransition(linkedDoor)
                );
            }
        }
        else
        {
            // Otros interactuables
            currentInteractive.OnInteraction();
        }
    }

    private void ShowInteractButton()
    {
        interactButton.gameObject.SetActive(true);
    }

    private void HideInteractButton()
    {
        interactButton.gameObject.SetActive(false);
    }

    public bool IfNeedToChangeButtonSpray()
    {
        return true;
    }


    void Update()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, maxDistance, interactableLayers))
        {
            IInteractive foundInteractive = hit.collider.GetComponent<IInteractive>();
            if (foundInteractive != null)
            {
                currentInteractive = foundInteractive;
                ShowInteractButton();
                return;
            }
        }

        currentInteractive = null;
        HideInteractButton();
    }
}
