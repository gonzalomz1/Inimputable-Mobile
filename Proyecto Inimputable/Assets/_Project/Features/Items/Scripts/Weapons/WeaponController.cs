using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public static WeaponController instance;

    [Header("WeaponModel")]
    public WeaponModel weaponModel;
    [Header("WeaponView")]
    public WeaponView weaponView;

    public event Action WeaponGameStartState;
    public event Action<WeaponType> OnWeaponPickedUp;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(gameObject);
        SubscribeToGameManagerEvents();
    }

    void Start()
    {
        SubscribeToActionCanvasEvents();
    }

    private void SubscribeToGameManagerEvents()
    {
        GameManager.instance.GameExecute += OnGameExecute;
    }

    void OnGameExecute()
    {
        StartWeapons();
    }

    private void StartWeapons()
    {
        WeaponGameStartState?.Invoke();
    }

    private void SubscribeToActionCanvasEvents()
    {
        PlayerActionsCanvas.Instance.stompRequest += OnStompRequest;
        PlayerActionsCanvas.Instance.shootRequest += OnShootRequest;
        PlayerActionsCanvas.Instance.reloadRequest += OnReloadRequest;
    }

    private void OnStompRequest() 
    {
        TryStomp();
    }

    private void OnShootRequest()
    {
        TryShoot();
    }

    private void OnReloadRequest()
    {
        TryReload();
    }


    public void PickUpWeapon(WeaponType type)
    {
        if (!weaponModel.pickedUpWeapons.Contains(type))
        {
            weaponModel.pickedUpWeapons.Add(type);
            weaponView.EquipWeapon(type);
            Debug.Log($"Weapon {type} picked up!");
            OnWeaponPickedUp?.Invoke(type);
        }
        OnNewWeaponStats();
    }

    public void OnNewWeaponStats()
    {
        Debug.Log($"Current Weapon: {weaponModel.currentWeapon}");
        Debug.Log("on Change UI Stats");
        WeaponObject newWeapon = weaponModel.GetCurrentWeapon();
        Debug.Log($"after call, newWeapon is: {newWeapon}");
        if (newWeapon.weaponType != WeaponType.Cane) weaponView.weaponStats.NewWeaponEquiped(newWeapon);
    }


    private void TryShoot() // called onClick() from ActionCanvas:ShootButton
    {
        if (weaponModel.currentWeaponObject != null)
        {

            if (weaponModel.isMeleeDraw)
            {
                weaponModel.cane.TriggerPull();
            }
            else
            {
                bool needToChange = weaponModel.currentWeaponObject.TriggerPull();
                if (needToChange)
                {
                    weaponView.ChangeUiCurrentAmmo();
                }
                else return;
            }

        }
    }

    private void TryReload() // called onClick() from ActionCanvas:ReloadButton
    {
        if (weaponModel.currentWeaponObject != null)
        {
            weaponModel.currentWeaponObject.Reload();
        }
    }

    private void TryStomp()
    {

    }

    public void ResetWeapons()
    {
        if (weaponModel != null) weaponModel.ResetWeaponState();
        if (weaponView != null) weaponView.DisableAllWeapons();
    }
}
