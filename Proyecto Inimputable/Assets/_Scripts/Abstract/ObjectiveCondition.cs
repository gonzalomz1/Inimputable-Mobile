using System;
using UnityEngine;

public abstract class ObjectiveCondition : MonoBehaviour
{
    protected ObjectiveBase _ownerObjective;

    public bool IsSatisfied { get; protected set; }

    // Bloquea cualquier trigger antes de Initialize()
    public bool _initialized = false;

    /// <summary>
    /// Llamado por el ObjectiveBase cuando comienza este objetivo.
    /// </summary>
    public virtual void Initialize(ObjectiveBase owner)
    {
        _ownerObjective = owner;
        _initialized = true;
        IsSatisfied = false;
        OnConditionInit();
    }

    /// <summary>
    /// Reinicia variables internas sin activar nada.
    /// </summary>
    public virtual void ResetCondition()
    {
        IsSatisfied = false;
        _initialized = false;
    }

    /// <summary>
    /// Limpia listeners y lo hecho en OnConditionInit.
    /// </summary>
    public virtual void Cleanup()
    {
        OnConditionCleanup();
        _initialized = false;
    }

    /// <summary>
    /// Llamado por las subclases cuando se cumple la condición.
    /// </summary>
    protected void MarkSatisfied()
    {
        if (!_initialized || IsSatisfied) return;

        IsSatisfied = true;

        // Avisar al objetivo que se cumplieron condiciones
        _ownerObjective.NotifyConditionSatisfied();
    }

    /// <summary>
    /// Gancho opcional para las subclases.
    /// </summary>
    protected virtual void OnConditionInit() { }

    /// <summary>
    /// Gancho opcional para limpieza.
    /// </summary>
    protected virtual void OnConditionCleanup() { }


    /// <summary>
    /// Protección total: si Unity reactiva por accidente este objeto,
    /// no ejecutará lógica a menos que Initialize() haya sido llamado.
    /// </summary>
    protected virtual void OnEnable()
    {
        if (!_initialized)
        {
            // no permitir ejecución fuera de tiempo
            return;
        }

        OnConditionEnabled();
    }

    /// <summary>
    /// Override opcional para condiciones que necesiten OnEnable (zonas, triggers, etc).
    /// </summary>
    protected virtual void OnConditionEnabled() { }
}
