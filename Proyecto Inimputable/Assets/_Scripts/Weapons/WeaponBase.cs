using UnityEngine;

public interface IWeapon
{
    void Fire();
    void Reload();
    bool CanFire { get; }
}

public interface IVisualWeapon
{

    void PlayMuzzleFlash();
    void UpdateUI(Sprite weaponSprite);

}
public abstract class WeaponBase : IWeapon
{
    protected float fireRate;
    protected int maxAmmo;
    protected int currentAmmo;
    protected float lastFireTime;

    public virtual bool CanFire => Time.time >= lastFireTime + fireRate && currentAmmo > 0;

    public virtual void Fire(){
        if (!CanFire) return;
        lastFireTime = Time.time;
        currentAmmo--;
        Shoot();
    }

    public abstract void Shoot();
    public abstract void Reload();
}
