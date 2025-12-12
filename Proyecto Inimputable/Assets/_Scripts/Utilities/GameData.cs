using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public LayerMask playerHitMask;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(gameObject);
    }


}