using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInventory : MonoBehaviour
{
    playerMenuManager playerMenuManager;
    PlayerCOntroller playerCOntroller;



    public int potionCount;






    void Start()
    {
        playerMenuManager = GetComponent<playerMenuManager>();
        playerCOntroller = GetComponent<PlayerCOntroller>();


        potionCount = 0;


    }

    void Update()
    {
        
    }
}
