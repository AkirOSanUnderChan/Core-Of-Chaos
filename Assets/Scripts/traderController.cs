using UnityEngine;

public class traderController : MonoBehaviour
{
    public Transform player;
    public float interactDistance = 2f;

    public GameObject InteractText;

    private bool showInteractPrompt = false;

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

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
