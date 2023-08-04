using System.Collections;
using System.Collections.Generic;
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

            //GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height - 50, 150, 30), "Press F to interact");
        }
        else
        {
            InteractText.SetActive(false);

        }
    }
}
