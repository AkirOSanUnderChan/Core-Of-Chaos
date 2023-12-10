using UnityEngine;

public class traderController : MonoBehaviour
{
    public PlayerCOntroller PlayerCOntroller;
    public float interactDistance = 2f;

    public GameObject InteractText;

    private bool showInteractPrompt = false;


    private void Start()
    {
        PlayerCOntroller = PlayerControllerSingleton.Instance.PlayerCOntroller;
    }
    private void Update()
    {
        float distance = Vector3.Distance(transform.position, PlayerCOntroller.transform.position);

        if (distance <= interactDistance)
        {
            showInteractPrompt = true;
        }
        else
        {
            showInteractPrompt = false;
        }

        
    }

    private void OnGUI()
    {
        if (showInteractPrompt)
        {
            InteractText.SetActive(true);

        }
        else
        {
            InteractText.SetActive(false);

        }
    }
}
