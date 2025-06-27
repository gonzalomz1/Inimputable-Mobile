using System.Collections.Generic;
using UnityEngine;

public class LightsManager : MonoBehaviour
{
    [SerializeField] Light centerLight;
    [SerializeField] private List<Light> doorLightsArray;


    void Start()
    {
        TurnOffLights();
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