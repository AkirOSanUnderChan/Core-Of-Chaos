using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class PlayerCOntroller : MonoBehaviour
{
    public int playerBalance = 0;

    private float nextRegenTime;


    public GameObject TraderWindow;

    public Transform playerCamera;
    public float interactDistance = 2f;

    public bool playerIsDead;

    public int maxHP = 20;
    public int currentHP;

    public int playerDamage;
    public int hpRegenAmount = 0;
    public float hpRegenInterval = 10f;

    public int playerCurrentXP;
    public float playerXPToLVLUP;
    public int playerLVL;
    public int playerUpgradeScore;

    public bool ground = false;

    public playerMenuManager playerMenuManager;
    public playerInventory playerInventory;


    public float fallDamageMultiplier = 0.2f;
    public float minimumFallSpeed = 10f;
    //GUI start
    public Slider playerXPBar;
    public TextMeshProUGUI hpCount;
    public Slider playerSliderHP;
    public TextMeshProUGUI PlayerXPbar_TEXT;
    public TextMeshProUGUI playerLVL_TEXT;



    //GUI end

    private void Start()
    {
        playerBalance = 0;

        TraderWindow.SetActive(false);
        playerIsDead = false;
        hpRegenInterval = 10f;
        playerDamage = 1;
        playerUpgradeScore = 0;
        currentHP = maxHP;
        playerLVL = 1;
        playerCurrentXP = 0;
        playerXPToLVLUP = 10f;
        playerMenuManager = GetComponent<playerMenuManager>();
        playerInventory = GetComponent<playerInventory>();

    }


    void Update()
    {
        if (Time.time >= nextRegenTime)
        {
            RegenerateHP();

            // Оновлення часу наступного виклику функції RegenerateHP
            nextRegenTime = Time.time + hpRegenInterval;
        }

        if (currentHP <= 0)
        {
            playerIsDead = true;
        }
        GuiUpdate();
        inputUpdate();
        lvlUpdate();





        if (gameObject != null)
        {
            // Якщо позиція гравця нижче -10, виконуємо певні дії
            if (transform.position.y < -10f)
            {
                currentHP = -5;
            }
        }

    }

    public void inputUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                if (hit.collider.CompareTag("NPC"))
                {
                    TraderWindow.SetActive(true);
                    playerMenuManager.playerInWindow = true;
                }
            }
        }





        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    animator.SetBool("atack", true);
        //    animator.SetBool("idle", false);
        //}
        //else
        //{
        //    animator.SetBool("atack", false);
        //    animator.SetBool("idle", true);
        //}


        if (Input.GetKeyDown(KeyCode.X))
        {
            if (playerInventory.potionCount >= 1)
            {
                playerInventory.potionCount--;
                currentHP += 20;
                if (currentHP > maxHP)
                {
                    currentHP = maxHP;
                }
            }
        }



    }
    public void lvlUpdate()
    {
        if (playerCurrentXP >= playerXPToLVLUP)
        {
            playerLVL++;
            playerCurrentXP = 0;
            playerXPToLVLUP = Mathf.Round(playerXPToLVLUP * 1.2f);
            playerUpgradeScore++;
        }
    }

    public void GuiUpdate()
    {
        PlayerXPbar_TEXT.SetText("XP: " + playerCurrentXP + " / " + playerXPToLVLUP);
        hpCount.SetText(currentHP.ToString() + " / " + maxHP.ToString());
        playerSliderHP.maxValue = maxHP;
        playerSliderHP.value = currentHP;
        playerLVL_TEXT.SetText("LVL:" + playerLVL);

        playerXPBar.maxValue = playerXPToLVLUP;
        playerXPBar.value = playerCurrentXP;


    }


    

    private void RegenerateHP()
    {
        // Перевірте, чи currentHP не перевищує максимальний HP
        if (currentHP < maxHP)
        {
            // Додайте кількість HP для відновлення
            currentHP += hpRegenAmount;

            // Перевірте, чи не перевищуємо максимальний HP
            if (currentHP > maxHP)
            {
                currentHP = maxHP;
            }
        }
    }



    




}
