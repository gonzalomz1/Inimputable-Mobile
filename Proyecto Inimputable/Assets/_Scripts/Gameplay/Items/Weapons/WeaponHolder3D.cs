using System;
using UnityEngine;

enum WeaponHolderState { Active, PickedUp, Destroyed }

public class WeaponHolder3D : ItemBase
{
    [SerializeField] private bool needToMakeAction = true;
    [SerializeField] private WeaponDataSO weaponDataSO;
    [SerializeField] private WeaponHolderState currentState;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private Rigidbody rigidBody;

    public event Action weaponPickedup;

    void Awake()
    {
        if (!weaponDataSO) return;
        if (!spriteRenderer) GetComponent<SpriteRenderer>();
        if (!sphereCollider) GetComponent<SphereCollider>();
        if (!rigidBody) GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        currentState = WeaponHolderState.Active;
        ManageState(currentState);
    }




    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentState = WeaponHolderState.PickedUp;
            ManageState(currentState);
            TriggerCallOnPickUp();
        }
    }


    void ManageState(WeaponHolderState current)
    {
        switch (current)
        {
            case WeaponHolderState.Active:
                SetSprite(weaponDataSO);
                break;
            case WeaponHolderState.PickedUp:
                SetAsPickedUpOnPlayer(weaponDataSO);
                currentState = WeaponHolderState.Destroyed;
                ManageState(currentState);
                break;
            case WeaponHolderState.Destroyed:
                // Destroy(this.gameObject);
                this.gameObject.SetActive(false); // Disable to allow reuse via Object Pooler
                break;
        }
    }

    void SetSprite(WeaponDataSO wd)
    {
        if (wd != null && spriteRenderer != null && spriteRenderer.sprite != wd.weaponIcon)
            spriteRenderer.sprite = wd.weaponIcon;
    }

    private void SetAsPickedUpOnPlayer(WeaponDataSO wd)
    {
        GameObject player = GameObject.FindWithTag("Player");
        WeaponController weaponController = player.GetComponent<WeaponController>();
        switch (wd.weaponName)
        {
            case "Pistol":
                weaponController.PickUpWeapon(WeaponType.Pistol);
                break;
        }
    }
}
