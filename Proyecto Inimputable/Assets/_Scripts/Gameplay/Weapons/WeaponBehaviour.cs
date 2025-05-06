using UnityEngine;

public abstract class WeaponBehaviour : MonoBehaviour
{
    protected WeaponData weaponData;
    protected Transform firePoint;

    public virtual void Initialize(WeaponData data, Transform firePoint)
    {
        this.weaponData = data;
        this.firePoint = firePoint;

    }

    public abstract void TriggerPull();     // llamada cuando el jugador intenta disparar
    public abstract void TriggerRelease();  // para armas automaticas
    public abstract void Reload();          // recarga segun tipo de arma

    public WeaponType GetWeaponType()
    {
        return weaponData.weaponType;
    }

    public void SetState(WeaponState state)
    {
        // Handle transitions of states or animations
    }
}
