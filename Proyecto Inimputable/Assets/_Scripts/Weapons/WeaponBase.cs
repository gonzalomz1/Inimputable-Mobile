using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// Cada arma debe tener:
/// - Su version en canvas visual
/// - Su almacenamiento de datos en esa version del canvas
/// -- el jugador tendra un componente de luz para simular el fogonaso.
/// -- este componente recibira datos como que arma es actualmente, y la posicion desde donde debe "salir"
/// - Su "version" para enemigo
///     Los enemigos tambien tendran armas con datos, aunque realmente sin valor agregado como tipo
///     sencillamente deben tambien tener armas ya que la base de todas las armas tendra un componente de luz para el fogonaso
/// - Maquina de estados segun esta "disparando" - "recargando"
/// - Version en el suelo
/// </summary>



public interface IWeapon
{
    public void Fire();
    public void Reload();

}

public abstract class WeaponBase : ScriptableObject, IWeapon
{

    public void Fire()
    {

    }

    public void Reload()
    {


    }


}
