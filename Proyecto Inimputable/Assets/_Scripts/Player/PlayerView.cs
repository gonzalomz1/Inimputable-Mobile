using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerView : MonoBehaviour
{

    public Rigidbody player;

    void Awake()
    {
        if (!player)
        {
            player = GetComponent<Rigidbody>();
        }
    }

    public void Move(Vector2 direction, float speed)
    {
        Vector3 move = new Vector3(direction.x, 0, direction.y);

        if (move.sqrMagnitude > 0.01f)
        {
            Vector3 newPosition = player.position + move * speed * Time.fixedDeltaTime;
            player.MovePosition(newPosition);


            /// Por ahora, no rotamos la camara
            // Rotar hacia la dirección de movimiento
            //Quaternion newRotation = Quaternion.LookRotation(move);
            //player.MoveRotation(newRotation);
        }
    }
}
