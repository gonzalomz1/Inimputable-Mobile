using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerView : MonoBehaviour
{
    public Rigidbody player;
    public Transform cameraPivot;
    public Transform cam;

    void Awake()
    {
        if (!player) player = GetComponent<Rigidbody>();
    }

    public void Move(Vector2 direction, float speed, Transform cameraTransform)
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * direction.y + right * direction.x).normalized;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Vector3 newPosition = player.position + moveDirection * speed * Time.fixedDeltaTime;
            player.MovePosition(newPosition);
        }
    }

    public void RotateCamera(float yaw, float pitch)
    {
        // Solo rotación de cámara
        //cameraPivot.localEulerAngles = new Vector3(pitch, 0f, 0f);
        cam.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}