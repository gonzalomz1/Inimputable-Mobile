using UnityEngine;

public class Pistol : WeaponBase, IVisualWeapon
{

public bool IsPlayer(Player prefab){
    if (prefab){
        return true;
    } else{
        return false;
    }
}




}
