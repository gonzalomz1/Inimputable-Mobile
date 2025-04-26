/*S — Single Responsibility Principle
"Una clase debe tener una única razón para cambiar."
Si cambia el sistema de movimiento o de animación, no afectan entre sí.

// SOLO se encarga de mover
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime);
    }
}

// SOLO se encarga de animar
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetRunning(bool isRunning)
    {
        animator.SetBool("IsRunning", isRunning);
    }
}


/*O — Open/Closed Principle
"El software debe estar abierto a extensión, pero cerrado a modificación."
Si queremos agregar una nueva arma como "Lanzallamas", no modificamos Weapon, solo extendemos.

public abstract class Weapon
{
    public abstract void Attack();
}

public class Sword : Weapon
{
    public override void Attack()
    {
        Debug.Log("Slash with sword!");
    }
}

public class Bow : Weapon
{
    public override void Attack()
    {
        Debug.Log("Shoot arrow!");
    }
}

/*
L — Liskov Substitution Principle
"Una clase derivada debe poder reemplazar a su base sin errores."
public class Player : MonoBehaviour
{
    public Weapon equippedWeapon;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            equippedWeapon.Attack();
        }
    }
}
/*
No importa si el arma es Sword o Bow, siempre puede Attack() sin problemas.

I — Interface Segregation Principle
"Muchas interfaces específicas son mejores que una sola general."

 Un ítem puede tener solo los métodos que necesita, no una interfaz gigante que incluya todo.

public interface IUsable
{
    void Use();
}

public interface IDroppable
{
    void Drop();
}

public class HealthPotion : MonoBehaviour, IUsable
{
    public void Use()
    {
        Debug.Log("Restoring health!");
    }
}

public class Sword : MonoBehaviour, IUsable, IDroppable
{
    public void Use()
    {
        Debug.Log("Attacking with sword!");
    }

    public void Drop()
    {
        Debug.Log("Dropped sword!");
    }
}

/*
D — Dependency Inversion Principle
"Las clases deben depender de abstracciones, no de clases concretas."
Podemos cambiar el arma sin que Player conozca los detalles de Gun, Sword, etc.

public interface IWeapon
{
    void Attack();
}

public class Gun : MonoBehaviour, IWeapon
{
    public void Attack()
    {
        Debug.Log("Bang bang!");
    }
}

public class Player : MonoBehaviour
{
    private IWeapon weapon;

    public void SetWeapon(IWeapon newWeapon)
    {
        weapon = newWeapon;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            weapon?.Attack();
        }
    }
}
*/