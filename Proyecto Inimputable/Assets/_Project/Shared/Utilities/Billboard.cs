using UnityEngine;

public class Billboard : MonoBehaviour
{
    public TurroController controller;
    public bool isEnemy;

    void Awake()
    {
        if (controller == null)
        {
            var miComponente = GetComponentInParent<TurroController>();
            if (miComponente != null)
            {
                controller = miComponente;
                isEnemy = true;
            }
            else
            {
                isEnemy = false;
            }
               
        }
    }

    void LateUpdate()
    {
        if (controller == null && isEnemy) return;
        if (isEnemy && controller.turroModel.turroState != EnemyState.Spawn) BillboardEffect();
        if (!isEnemy && controller == null) BillboardEffect();
    }

    private void BillboardEffect()
    {
        transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
    }

}

