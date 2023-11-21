using UnityEngine;

public class PlayerControllerSingleton : MonoBehaviour
{
    public static PlayerControllerSingleton Instance { get; private set; }
    public PlayerCOntroller PlayerCOntroller { get; internal set; }

    public PlayerCOntroller PlayerController;
    public playerMenuManager playerMenuManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
