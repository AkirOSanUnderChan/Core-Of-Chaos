using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerMenuManager : MonoBehaviour
{
    InventoryManager inventoryManager;
    public handsAnim handsAnimScript;
    public Item goldCoinItem;
    public Item potionItem;
    public bool playerInWindow = false;

    public AudioSource menuAudioSourse;
    public AudioClip succsecfullBuy;

    public bool debugMod = false;
    public GameObject Graphy;

    public TextMeshProUGUI UpgradePoints_UI;

    public TextMeshProUGUI MaxHPStats_Menu;
    public TextMeshProUGUI StaminaStatus_Menu;
    public TextMeshProUGUI DamageStatus_Menu;
    public TextMeshProUGUI RegenerationStatus_Menu;
    public GameObject DeathMenu;
    public GameObject PauseMenu;
    public GameObject InventoryObject;
    public TextMeshProUGUI potionCount_UI;
    public TextMeshProUGUI playerBalance_UI;
    public TextMeshProUGUI playerBalanceInScreen_UI;

    public TextMeshProUGUI playerInventoryLvl_UI;


    public TextMeshProUGUI playerEnergi_UI;
    public Slider playerEnergiSlider_UI;

    public PlayerCOntroller ggControll;
    public playerInventory playerInventory;


    public GameObject hotbarGroop;
    public GameObject mainHudGroop;

    public GameObject blockGroop;
    public Slider blockSlider;

    public GameObject playerMenu;
    public bool PM_IsActive;
    public bool Inventory_IsActive;
    public bool isPaused = false;
    public bool playerInPause = false;

    



    private void Start()
    {
        playerInWindow = false;
        playerInPause = false;
        Inventory_IsActive = false;

        DeathMenu.SetActive(false);
        inventoryManager = GetComponent<InventoryManager>();
        handsAnimScript = PlayerControllerSingleton.Instance.handsAnimScript;
        ggControll = GetComponent<PlayerCOntroller>();
        playerInventory = GetComponent<playerInventory>();

        playerMenu.SetActive(false);
        PauseMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        blockSlider.maxValue = 100;
        blockSlider.minValue = 0;
    }
    private void Update()
    {
        




        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }


        if (playerInWindow)
        {
            isPaused = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (!playerInWindow)
        {
            isPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        menuGuiUpdate();


        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!ggControll.playerIsDead)
            {
                if (playerInWindow)
                {
                    playerMenu.SetActive(false);
                    PM_IsActive = false;
                    ggControll.TraderWindow.SetActive(false);
                    PauseMenu.SetActive(false);
                    playerInWindow = false;
                    playerInPause = false;
                    InventoryObject.SetActive(false);
                    mainHudGroop.SetActive(true);
                    hotbarGroop.SetActive(true);



                }
                else
                {
                    PauseMenu.SetActive(true);
                    playerInWindow = true;
                    playerInPause = true;
                    
                }
            }
            

        }

        if (ggControll.playerIsDead)
        {
            ggControll.playerIsDead = true;
            DeathMenu.SetActive(true);
            playerMenu.SetActive(false);
            playerInWindow = true;
            playerInPause = true;
            mainHudGroop.SetActive(false);
            hotbarGroop.SetActive(false);
            InventoryObject.SetActive(false);


        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            DebugMod();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!ggControll.playerIsDead)
            {
                if (!playerInPause)
                {
                    if (Inventory_IsActive)
                    {
                        InventoryObject.SetActive(false);

                        mainHudGroop.SetActive(true);
                        hotbarGroop.SetActive(true);

                        PM_IsActive = false;
                        playerInWindow = false;
                        Inventory_IsActive = false;


                    }
                    else if (!Inventory_IsActive)
                    {
                        InventoryObject.SetActive(true);

                        playerMenu.SetActive(false);
                        mainHudGroop.SetActive(false);
                        hotbarGroop.SetActive(false);

                        PM_IsActive = false;
                        Inventory_IsActive = true;
                        playerInWindow = true;
                    }
                }
                else
                {
                    playerMenu.SetActive(false);
                    InventoryObject.SetActive(false);

                    mainHudGroop.SetActive(false);
                    hotbarGroop.SetActive(false);
                }
                
            }
        }

        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    if (!ggControll.playerIsDead)
        //    {
        //        if (!playerInPause)
        //        {
        //            if (PM_IsActive)
        //            {
        //                InventoryObject.SetActive(false);
        //                playerMenu.SetActive(false);

        //                mainHudGroop.SetActive(true);
        //                hotbarGroop.SetActive(true);

        //                PM_IsActive = false;
        //                playerInWindow = false;
        //            }
        //            else if (!PM_IsActive)
        //            {
        //                InventoryObject.SetActive(false);

        //                playerMenu.SetActive(true);

        //                mainHudGroop.SetActive(false);
        //                hotbarGroop.SetActive(false);

        //                PM_IsActive = true;
        //                playerInWindow = true;
        //                Inventory_IsActive = false;

        //            }
        //        }

        //    }
        //}

    }
    public void PlayerInGame()
    {
        playerInWindow = false;
    }
    public void buyHealPotion()
    {
        bool canBuy = InventoryManager.instance.DeleteTheSpecifiedItem(goldCoinItem, 5);
        if (canBuy)
        {
            menuAudioSourse.PlayOneShot(succsecfullBuy);
            InventoryManager.instance.AddItem(potionItem, 1);
        }
        else
            Debug.Log("У вас не вистачає грошей, щоб купити цей предмет " + potionItem.itemName + " Ціна якого " + 5 + " " + goldCoinItem.itemName);
    }


    public void maxHealthUpgrade()
    {


        if (ggControll.playerUpgradeScore >= 1)
        {
            ggControll.playerUpgradeScore--;
            ggControll.maxHP += 10;
        }



    }
    public void StaminaUpgrade()
    {
        if (ggControll.playerUpgradeScore >= 1)
        {
            ggControll.playerUpgradeScore--;
            ggControll.maxEnergi += 10;
            ggControll.energiRegenSpeed += 1;
        }
    }
    public void DamageUpgrade()
    {
        if (ggControll.playerUpgradeScore >= 1)
        {
            ggControll.playerUpgradeScore--;
            ggControll.playerDamage++;
        }
    }
    public void RegenUpgrade()
    {
        if (ggControll.playerUpgradeScore >= 1)
        {
            ggControll.playerUpgradeScore--;
            ggControll.hpRegenAmount++;
            ggControll.hpRegenInterval -= 1f;
            if (ggControll.hpRegenInterval < 1f)
            {
                ggControll.hpRegenInterval = 1f;
            }
        }
    }

    public void restartButton()
    {
        SceneManager.LoadScene(1);
    }
    public void quitGameButton()
    {
        Application.Quit();
    }
    public void mainMenuButton()
    {
        SceneManager.LoadScene(0);
        //////////////////////////////

    }
    public void ResumeButton()
    {
        PauseMenu.SetActive(false);
        playerInWindow = false;
        playerInPause = false;


    }

    public void DebugMod()
    {
        debugMod = !debugMod;
        Graphy.SetActive(debugMod);
    }

    public void menuGuiUpdate()
    {
        UpgradePoints_UI.SetText("Upgrade points " + ggControll.playerUpgradeScore.ToString());
        MaxHPStats_Menu.SetText("Max health: " + ggControll.maxHP);
        //SpeedStatus_Menu.SetText("Run speed: " + ggControll.runSpeed);
        DamageStatus_Menu.SetText("Damage: " + ggControll.playerDamage);
        RegenerationStatus_Menu.SetText("Regeration: " + ggControll.hpRegenAmount);
        potionCount_UI.SetText(playerInventory.potionCount.ToString());
        playerBalance_UI.SetText("Your balance: " + ggControll.playerBalance.ToString());
        playerBalanceInScreen_UI.SetText("Balance: " + ggControll.playerBalance.ToString());
        playerInventoryLvl_UI.SetText("Level: " + ggControll.playerLVL.ToString());
        StaminaStatus_Menu.SetText("Stamina: " + ggControll.maxEnergi.ToString());

        playerEnergi_UI.SetText($"{ggControll.currentEnergi:F0}/{ggControll.maxEnergi:F0}");

        playerEnergiSlider_UI.maxValue = ggControll.maxEnergi;
        playerEnergiSlider_UI.value = ggControll.currentEnergi;

        blockSlider.value = handsAnimScript.blockTime;
        if (handsAnimScript.blockTime >= 100)
        {
            blockGroop.SetActive(false);
        }
        else
        {
            blockGroop.SetActive(true);
        }


    }
}
