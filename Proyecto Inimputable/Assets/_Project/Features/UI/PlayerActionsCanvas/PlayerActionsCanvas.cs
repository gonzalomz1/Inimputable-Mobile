using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using System.Collections.Generic;


public class PlayerActionsCanvas : MonoBehaviour
{
    public static PlayerActionsCanvas Instance { get; private set; }
    [Header("Buttons")]
    public Button shootButton;
    public Button reloadButton;
    public Button pauseButton;
    public Button interactButton;
    public Button stompButton;
    private bool isShowingInteract = false;
    [SerializeField] private List<Sprite> shootSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> reloadSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> pauseSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> interactSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> stompSprites = new List<Sprite>();

    [Header("Interaction Settings")]
    public float maxDistance = 2;
    public LayerMask interactableLayers;
    public LayerMask stopmLayer;
    private IInteractive currentInteractive;
    [Header("References")]
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    public WeaponController weaponController;

    public Camera playerCamera;

    public event Action pauseRequest;
    public event Action stompRequest;
    public event Action reloadRequest;
    public event Action shootRequest;

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
                if (button == stompButton) button.image.sprite = stompSprites[1];
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
        if (stompButton.image.sprite == stompSprites[1]) stompButton.image.sprite = stompSprites[0];
    }

    public void OnClickReload()
    {
        reloadRequest?.Invoke();
    }
    public void OnClickShoot()
    {
        shootRequest?.Invoke();
    }
    public void OnClickPause()
    {
        pauseRequest?.Invoke();
    }
    public void OnClickStopm()
    {
        stompRequest?.Invoke();
    }

    public void Interact()
    {
        if (currentInteractive == null) return;

        if (currentInteractive is Door door)
        {
            if (door.TryToOpenDoor(out Door linkedDoor))
            {
                GameManager.instance.StartCoroutine(
                   PlayerTeleporter.instance.DoDoorTransition(linkedDoor)
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

    private void HideStompButton()
    {
        stompButton.gameObject.SetActive(false);
    }

    private void HideAllButtons()
    {
        HideInteractButton();
        HideStompButton();
    }

    public bool IfNeedToChangeButtonSpray()
    {
        return true;
    }

    void Update()
    {
        if (playerCamera == null) return; // Basic safety check

        // Check forward from the camera to see if we're looking at something interactable (like a Door)
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, maxDistance, interactableLayers))
        {
            // Debug.Log($"Raycast Hit: {hit.collider.name} on Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
            IInteractive foundInteractive = hit.collider.GetComponent<IInteractive>();
            if (foundInteractive != null)
            {
                currentInteractive = foundInteractive;
                ShowInteractButton();
                return;
            }
            else
            {
                 // Hit something, but it didn't have the IInteractive component
            }
        }
        else
        {
             // Raycast didn't hit anything in range
        }

        currentInteractive = null;
        HideAllButtons();
    }
}
