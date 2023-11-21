using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class cameracontroll : MonoBehaviour
{
    

    private PlayerCOntroller playerController;

    private void Start()
    {
        // Знаходимо скрипт PlayerController на гравцеві
        playerController = FindObjectOfType<PlayerCOntroller>();
    }

    private void Update()
    {
        
    }
}