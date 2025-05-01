using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : CustomCanvas

{
    public TextMesh play;
    public TextMesh option;
    public TextMesh credits;
    public TextMesh exit;

    public override void SetActiveCanvas(bool isActive)
    {
        gameObject.SetActive(isActive); // Cambia la visibilidad del canvas
    }
   public void SetTextMeshTransform(List<Vector2>list ){
    Debug.Log(list);
    

   }
   
}
