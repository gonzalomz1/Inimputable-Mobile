using Unity.VisualScripting;
using UnityEngine;

public class EventMessager : MonoBehaviour
{
    public TurroController turroController;
    public TurroModel turroModel;
    public TurroView turroView;

    void Awake()
    {
        if (turroController == null)
        {
            var miController = GetComponentInParent<TurroController>();
            if (miController != null) turroController = miController;
        }
        if (turroModel == null)
        {
            var miModel = GetComponentInParent<TurroModel>();
            if (miModel != null) turroModel = miModel;
        }
        if (turroView == null)
        {
            var miView = GetComponentInParent<TurroView>();
            if (miView != null) turroView = miView;
        }
    }

    public void TriggerIdleState()
    {
        turroController.SetState(EnemyState.Idle);
        turroController.turroState = EnemyState.Idle;

    }

    public void EnableChase()
    {
        Debug.Log($"canChace: {turroModel.canChase}");
        turroModel.canChase = true;
        Debug.Log($"canChace: {turroModel.canChase}");
    }

    public void ShowSprite()
    {
        turroView.ShowSprite();
    }


    public void TriggerDieState()
    {
        turroController.SetState(EnemyState.Die);
        turroController.turroState = EnemyState.Die;

    }

    public void TriggerDestroyState()
    {
        turroController.SetState(EnemyState.Destroy);
        turroController.turroState = EnemyState.Destroy;
    }
}
