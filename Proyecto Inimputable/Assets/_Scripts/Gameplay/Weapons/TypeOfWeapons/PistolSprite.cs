using UnityEngine;

public class PistolSprite : MonoBehaviour
{
    [SerializeField] private Pistol parent;
    void Awake()
    {
        parent = GetComponentInParent<Pistol>();
    }

    public void FinishReload(){
        parent.FinishReload();
    }

}
