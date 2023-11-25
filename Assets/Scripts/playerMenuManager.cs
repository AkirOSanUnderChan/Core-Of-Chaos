using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerMenuManager : MonoBehaviour
{
    public bool playerInWindow = false;

    public bool debugMod = false;
    public GameObject Graphy;

    public TextMeshProUGUI UpgradePoints_UI;

    public TextMeshProUGUI MaxHPStats_Menu;
    public TextMeshProUGUI SpeedStatus_Menu;
    public TextMeshProUGUI DamageStatus_Menu;
    public TextMeshProUGUI RegenerationStatus_Menu;
    public GameObject DeathMenu;
    public GameObject PauseMenu;
    public TextMeshProUGUI potionCount_UI;
    public TextMeshProUGUI playerBalance_UI;
    public TextMeshProUGUI playerBalanceInScreen_UI;


    public TextMeshProUGUI playerEnergi_UI;
    public Slider playerEnergiSlider_UI;

    public PlayerCOntroller ggControll;
    public playerInventory playerInventory;

    public GameObject playerMenu;
    public bool PM_IsActive;
    public bool isPaused = false;
    public bool playerInPause = false;



    private void Start()
    {
        playerInWindow = false;
        playerInPause = false;

        DeathMenu.SetActive(false);
        ggControll = GetComponent<PlayerCOntroller>();
        playerInventory = GetComponent<playerInventory>();

        playerMenu.SetActive(false);
        PauseMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;

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

        }else if (!playerInWindow)
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
                    if (PM_IsActive)
                    {
                        playerMenu.SetActive(false);
                        PM_IsActive = false;
                        playerInWindow = false;
                    }
                    else if (!PM_IsActive)
                    {
                        playerMenu.SetActive(true);
                        PM_IsActive = true;
                        playerInWindow = true;
                    }
                }
                
            }
            
        }
    }
    public void PlayerInGame()
    {
        playerInWindow = false;
    }

    public void buyHealPotion()
    {
        if (ggControll.playerBalance >= 5)
        {
            ggControll.playerBalance -= 5;
            playerInventory.potionCount ++;
        }
    }


    public void maxHealthUpgrade()
    {
        if (ggControll.playerUpgradeScore >= 1)
        {
            ggControll.playerUpgradeScore --;
            ggControll.maxHP++;
        }
    }
    //public void SpeedUpgrade()
    //{
    //    if (ggControll.playerUpgradeScore >= 1)
    //    {
    //        ggControll.playerUpgradeScore--;
    //        ggControll.runSpeed++;
    //    }
    //}
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



        playerEnergi_UI.SetText(ggControll.currentEnergi.ToString() + " / " + ggControll.maxEnergi.ToString());

        playerEnergiSlider_UI.maxValue = ggControll.maxEnergi;
        playerEnergiSlider_UI.value = ggControll.currentEnergi;

    }
}
