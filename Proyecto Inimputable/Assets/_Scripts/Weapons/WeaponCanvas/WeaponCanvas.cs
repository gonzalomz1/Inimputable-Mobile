using UnityEngine;

public class WeaponCanvas : CustomCanvas // Representa la parte visual del arma
{
    
    public WeaponManager weaponManager;

   public override void SetActiveCanvas(bool isActive){
    gameObject.SetActive(isActive);
   }
    

}
