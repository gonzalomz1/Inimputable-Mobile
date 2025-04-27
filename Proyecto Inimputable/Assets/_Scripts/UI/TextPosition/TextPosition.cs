using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPosition : MonoBehaviour
{
    public MenuScreen menuScreen;
    public MenuCamera menuCamera;
    private Camera cameraComponent;
    private RectTransform menuScreenRectTransform;

    void Start()
    {
        if (menuCamera != null){
            cameraComponent = menuCamera.gameObject.GetComponent<Camera>();
        }
        if (menuScreen != null){
            menuScreenRectTransform = menuScreen.gameObject.GetComponent<RectTransform>();

        }
    }
    public void TransformPositionToCanvasPosition()
    {

        Vector3 worldPos = this.transform.position; // La posición 3D en el mundo
        Vector3 screenPos = cameraComponent.WorldToScreenPoint(worldPos); // Convierte a pantalla
        Debug.Log(screenPos);

        /*Vector2 canvasPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            menuScreenRectTransform, // El RectTransform del Canvas
            screenPos,           // La posición en pantalla
            Camera.main,         // La cámara usada
            out canvasPos        // La posición en el Canvas
        );*/

        // Ahora canvasPos tiene la posición en 2D para el Canvas
    }
    
}
