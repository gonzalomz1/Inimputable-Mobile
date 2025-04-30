using UnityEngine;

enum WeaponHolderState { Active, PickedUp, Destroyed }
public class WeaponHolder3D : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;

    [SerializeField] private WeaponHolderState currentState = WeaponHolderState.Active;

    void Awake()
    {
        if (!weaponData) return;
    }

    void Start()
    {
        ManageState(currentState);
    }

    void ManageState(WeaponHolderState current)
    {
        switch (current)
        {
            case WeaponHolderState.Active:
                break;
            case WeaponHolderState.PickedUp:
                break;
            case WeaponHolderState.Destroyed:
                break;
        }
    }

    void SetTexture(WeaponData wd)
    {

    }


}
