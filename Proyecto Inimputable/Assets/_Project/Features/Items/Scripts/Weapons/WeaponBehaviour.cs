using UnityEngine;

public abstract class WeaponBehaviour : ItemBase
{
    protected WeaponDataSO weaponDataSO;
    protected Transform firePoint;

    public virtual void Initialize(WeaponDataSO data, Transform firePoint)
    {
        this.weaponDataSO = data;
        this.firePoint = firePoint;

    }

    public abstract bool TriggerPull();     // llamada cuando el jugador intenta disparar
    public abstract void TriggerRelease();  // para armas automaticas
    public abstract void Reload();          // recarga segun tipo de arma

    public WeaponType GetWeaponType()
    {
        return weaponDataSO.weaponType;
    }

}
