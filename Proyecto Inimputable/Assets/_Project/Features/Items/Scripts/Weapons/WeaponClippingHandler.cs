using UnityEngine;

public class WeaponClippingHandler : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform weaponTransform;
    [SerializeField] private LayerMask collisionMask;

    [Header("Pushback Settings")]
    [SerializeField] private float checkDistance = 0.5f;
    [SerializeField] private float pushbackAmount = 0.3f;
    [SerializeField] private float smoothSpeed = 10f;

    private Vector3 defaultLocalPosition;
    private Vector3 targetLocalPosition;


    private void Start()
    {
        if (weaponTransform != null)
            defaultLocalPosition = weaponTransform.localPosition;
    }

    private void Update()
    {
        bool isClipping = Physics.Raycast(
            cameraTransform.position,
            cameraTransform.forward,
            out RaycastHit hit,
            checkDistance,
            collisionMask);

        if (isClipping)
        {
            targetLocalPosition = defaultLocalPosition - new Vector3(0f, 0f, pushbackAmount);
        }
        else
        {
            targetLocalPosition = defaultLocalPosition;
        }

        if (weaponTransform != null)
        {
            weaponTransform.localPosition = Vector3.Lerp(
                weaponTransform.localPosition,
                targetLocalPosition,
                Time.deltaTime * smoothSpeed
            );
        }
            
    } 
}