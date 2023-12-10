using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Input = UnityEngine.Input;

public class PlayerCOntroller : MonoBehaviour
{
    InventoryManager inventoryManager;
    public Item healingPotion;
    public int playerBalance = 0;

    private float nextRegenTime;
    public float maxDash;
    public float smoothDashSpeed;
    public bool canDash = true;
    public float dashCooldown; // Затримка між використанням дешу в секундах

    public TextMeshProUGUI pickupText;

    public GameObject TraderWindow;

    public Transform playerCamera;
    public float interactDistance = 2f;

    public bool playerIsDead;

    public int maxHP = 20;
    public int currentHP;
    public int hpRegenAmount = 0;
    public float hpRegenInterval = 10f;

    public float maxEnergi;
    public float currentEnergi;
    public float energiRegenSpeed;
    public float needEnergiForDash;

    public int playerDamage;


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

    public bool canTakeDamage;



    //GUI end

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        canTakeDamage = true;

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

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }


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
            Ray TradeRay = new Ray(playerCamera.position, playerCamera.forward);
            RaycastHit TradeHit;

            if (Physics.Raycast(TradeRay, out TradeHit, interactDistance))
            {
                if (TradeHit.collider.CompareTag("NPC"))
                {
                    TraderWindow.SetActive(true);
                    playerMenuManager.playerInWindow = true;
                }
            }
        }

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5))
        {
            // Перевіряємо, чи торкнувся райкаст якого-небудь об'єкта
            if (hit.collider != null)
            {
                // Перевіряємо, чи цей об'єкт має компонент інвентарю (може бути вашим скриптом)
                ItemPickUp itemPickUp = hit.collider.GetComponent<ItemPickUp>();

                if (itemPickUp != null)
                {
                    // Відображення інструкції для збору предмету
                    pickupText.text = "Press F to pick up the " + itemPickUp.item.itemName + " " + itemPickUp.currentStack + "x";

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        itemPickUp.PickupThisItem();
                    }
                }
                else
                {
                    // Якщо об'єкт не має компонента інвентарю, приховати текст
                    pickupText.text = "";
                }
            }
        }
        else
        {
            // Якщо не торкнуто об'єкт, також приховати текст
            pickupText.text = "";
        }


        if (Input.GetKeyDown(KeyCode.X))
        {
            bool prefect = InventoryManager.instance.DeleteTheSpecifiedItem(healingPotion, 1);
            if (prefect)
            {
                useHealingPotion();
                InventoryManager.instance.UpdateInventoryUI();
            }

        }

        if (Input.GetKey(KeyCode.LeftControl) & currentEnergi > 0)
        {
            currentEnergi -= 20 * Time.deltaTime;
        }
        else if(!Input.GetKey(KeyCode.LeftControl))
        {
            if (currentEnergi < maxEnergi)
            {
                currentEnergi += 10 * energiRegenSpeed * Time.deltaTime;
                if (currentEnergi > maxEnergi)
                {
                    currentEnergi = maxEnergi;
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

    public void ClaimExperience(int droppedXP)
    {
        int playerRemainsToLVLUP = Convert.ToInt32(playerXPToLVLUP) - playerCurrentXP;

        if (droppedXP >= playerRemainsToLVLUP)
        {
            
            playerLVL++;
            playerUpgradeScore++;
            droppedXP -= playerRemainsToLVLUP;
            
            playerCurrentXP = 0;
            playerXPToLVLUP = CalculateXPToLevelUp(); 
            
            ClaimExperience(droppedXP);
        }
        else
        {
            playerCurrentXP += droppedXP;
        }
    }

    public float CalculateXPToLevelUp()
    {
        playerXPToLVLUP = Mathf.Round(playerXPToLVLUP * 1.2f);
        return playerXPToLVLUP;

    }

    // Приклад виклику функції:
    // int droppedXP = // Ваша кількість отриманого досвіду
    // player.ClaimExperience(droppedXP);




    public void usingItem(Item itemToUse)
    {
        switch (itemToUse.itemName)
        {
            case "Healing potion":
                useHealingPotion();
                break;
        }
    }


    public void useHealingPotion()
    {
        currentHP += 20;
    }


}
