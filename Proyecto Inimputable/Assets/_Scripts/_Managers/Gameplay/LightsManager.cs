using System.Collections.Generic;
using UnityEngine;

public class LightsManager : MonoBehaviour
{
    public static LightsManager Instance { get; private set; }

    [SerializeField] Light centerLight;
    [SerializeField] private List<Light> doorLightsArray;

   private void Awake()
   {
      if (Instance != null && Instance != this)
      {
         Destroy(gameObject);
         return;
      }
      Instance = this;
   }

    public void TurnOffLights()
    {
        foreach (Light l in doorLightsArray)
        {
            l.gameObject.SetActive(false);
        }
    }

    public void TurnOnLights()
    {
        foreach (Light l in doorLightsArray)
        {
            l.gameObject.SetActive(true);
        }
    }

}