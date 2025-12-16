using UnityEngine;
using System.Collections;
using System;

public class PlayerTeleporter : MonoBehaviour
{
    public static PlayerTeleporter instance;
    
    public event Action DisablePlayerForTeleport;
    public event Action EnablePlayerAfterTeleport;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(gameObject);
    }

    public IEnumerator DoDoorTransition(Door linkedDoor)
    {
        GameManager.instance.FadeInScreen();
        GameManager.instance.DisableInput();
        GameManager.instance.HideAllCanvas();
        GameManager.instance.TriggerDoorTransitionSound();
        DisablePlayerForTeleport?.Invoke();
        
        if (linkedDoor != null)
        {
            PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.Teleport(linkedDoor.GetPlayerSpawnPosition(), linkedDoor.GetSpawnRotation());
            }
        }

        yield return new WaitForSeconds(0.5f);
        EnablePlayerAfterTeleport?.Invoke();
        GameManager.instance.EnableInput();
        GameManager.instance.FadeOutScreen();
        GameManager.instance.ShowAllCanvas();
    }
}