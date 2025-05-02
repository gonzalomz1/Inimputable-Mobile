using System.Net;
using System.Runtime.CompilerServices;
using UnityEngine;

enum WeaponHolderState { Active, PickedUp, Destroyed }
public class WeaponHolder3D : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private WeaponHolderState currentState;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private WeaponManager weaponManager;

    void Awake()
    {
        if (!weaponData) return;
        if (!spriteRenderer) GetComponent<SpriteRenderer>();
        if (!sphereCollider) GetComponent<SphereCollider>();
        if (!rigidBody) GetComponent<Rigidbody>();
    }

    void Start()
    {
        currentState = WeaponHolderState.Active;
        ManageState(currentState);
        
    }

    void ManageState(WeaponHolderState current)
    {
        switch (current)
        {
            case WeaponHolderState.Active:
                SetSprite(weaponData);
                break;
            case WeaponHolderState.PickedUp:
                GiveDataToWeaponManager(weaponData);
                currentState = WeaponHolderState.Destroyed;
                ManageState(currentState);
                break;
            case WeaponHolderState.Destroyed:
                Destroy(this.gameObject);
                break;
        }
    }

    void SetSprite(WeaponData wd)
    {
        if (wd && spriteRenderer)
        {
            spriteRenderer.sprite = wd.sprite;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player"){
            currentState = WeaponHolderState.PickedUp;
            ManageState(currentState);
        } 
        
    }

    private void GiveDataToWeaponManager(WeaponData wd){
        if (!weaponManager)Debug.LogError("NO WeaponManager available in reference for WeaponHolder3D.");
        WeaponData duplicate = ScriptableObject.Instantiate(wd);
        weaponManager.SetWeaponSlot(duplicate);
    }


}
