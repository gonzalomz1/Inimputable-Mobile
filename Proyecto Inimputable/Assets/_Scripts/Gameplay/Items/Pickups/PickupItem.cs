using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public enum PickupType { Health, Ammo }
    public PickupType type;
    public int amount = 20;
    [Header("Visuals")]
    public SpriteRenderer visualRenderer;
    public Sprite healthSprite;
    public Sprite ammoSprite;

    // Simple bobbing/rotating effect
    public float rotationSpeed = 50f;
    public float bobSpeed = 2f;
    public float bobHeight = 0.2f;

    private Vector3 startPos;

    void OnEnable()
    {
        startPos = transform.position;
    }

    public void Configure(PickupType newType)
    {
        type = newType;
        if (visualRenderer != null)
        {
            visualRenderer.sprite = (type == PickupType.Health) ? healthSprite : ammoSprite;
        }
    }

    void Update()
    {
        // Simple visual feedback
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            bool collected = false;
            
            if (type == PickupType.Health)
            {
                 PlayerData playerData = other.GetComponent<PlayerData>();
                 if (playerData != null && playerData.currentHealth < playerData.maxHealth)
                 {
                     playerData.Heal(amount);
                     collected = true;
                 }
            }
            else if (type == PickupType.Ammo)
            {
                Debug.Log($"[PickupItem] Type is Ammo. Trying to add {amount} ammo.");
                // Access WeaponController -> Current Weapon -> Add Ammo
                WeaponController weaponController = WeaponController.instance;
                if (weaponController == null) Debug.LogError("[PickupItem] WeaponController instance is NULL!");
                
                if (weaponController != null)
                {
                    if (weaponController.weaponModel == null) Debug.LogError("[PickupItem] WeaponModel is NULL!");
                    else if (weaponController.weaponModel.currentWeaponObject == null) Debug.LogError("[PickupItem] CurrentWeaponObject is NULL!");

                    if (weaponController.weaponModel != null && weaponController.weaponModel.currentWeaponObject != null)
                    {
                        WeaponObject weapon = weaponController.weaponModel.currentWeaponObject;
                        Debug.Log($"[PickupItem] Current Weapon is: {weapon.name}, Type: {weapon.weaponType}");
                        
                        if (weapon.weaponType != WeaponType.Cane && weapon.weaponType != WeaponType.Cane) 
                        {
                             Debug.Log("[PickupItem] Weapon is compatible. Adding Ammo...");
                             weapon.AddAmmo(amount);
                             collected = true;
                        }
                        else
                        {
                            Debug.Log("[PickupItem] Cannot add ammo to Melee/Cane.");
                        }
                    }
                }
            }

            if (collected)
            {
                // Return to pool instead of destroying
                gameObject.SetActive(false);
            }
        }
    }
}
