using UnityEngine;

/// <summary>
/// manejamos las armas del jugador aca. 
/// nos comunicamos con la visual del jugador (WeaponCanvas)
/// organizamos los datos necesarios (referencias a los WeaponData(cada arma))
/// Recibimos input del jugador y lo traducimos a logicas necesarias (visual y datos).
/// </summary>

enum WeaponState
{
    Idle,
    Drawing,
    Firing,
    Reloading,
    Empty
}

public class WeaponManager : MonoBehaviour 
{

    public WeaponData pistolSlot;
    private bool pistol = false;
    public WeaponData rifleSlot;
    private bool rifle = false;
    public WeaponData currentWeapon;

    public void SetWeaponSlot(WeaponData wd){
        switch (CheckWeaponType(wd)){
            case "pistolSlot":
                if (!pistol) 
                    {
                    pistolSlot = wd;
                    pistol = true;
                    }
                else AddAmmoReserve(pistolSlot, wd);
                break;
            case "rifleSlot":
                if (!rifle) {
                    rifleSlot = wd;
                    rifle = true;
                    }
                else AddAmmoReserve(pistolSlot, wd);
                break;
        }
    }

    public string CheckWeaponType(WeaponData wd){
        switch (wd.weaponName){
            case "Pistol":
                return "pistolSlot";
            case "Rifle":
                return "rifleSlot";
        }
        return "";
    }



    public void AddAmmoReserve(WeaponData slot, WeaponData wd){
        slot.ammoReserve += wd.ammoPickupSize;
        if (slot.ammoReserve >= wd.maxAmmoReserve) slot.ammoReserve = wd.maxAmmoReserve;
    }



}