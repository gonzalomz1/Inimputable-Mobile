using UnityEngine;

public class PistolSprite : MonoBehaviour
{
    [SerializeField] private WeaponObject parent;
    void Awake()
    {
        parent = GetComponentInParent<WeaponObject>();
    }

    public void OnReloadAnimationFinish()
    {
        parent.OnReloadAnimationFinish();
    }

    public void FinishShooting()
    {
        parent.FinishShooting();
    }

    public void BackToIdle()
    {
        parent.BackToIdle();
    }
    
    public void CheckEmptyAmmo()
    {
        parent.CheckEmptyAmmo();
    }

    public void EnableFlash()
    {
        parent.EnableFlash();
    }

    public void DisableFlash()
    {
        parent.DisableFlash();
    }
}
