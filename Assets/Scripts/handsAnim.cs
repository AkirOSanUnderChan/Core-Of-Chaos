using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handsAnim : MonoBehaviour
{
    private PlayerCOntroller playerController;


    private playerMenuManager playerMenuManager;
    public Animator animator;



    public float comboTime = 0.1f;

    public int comboStreek;
    public float blockTime = 100;
    public float blockDelay;
    public bool canBlock;

    // Start is called before the first frame update
    void Start()
    {
        comboStreek = 0;

        animator = GetComponent<Animator>();
        playerMenuManager = PlayerControllerSingleton.Instance.playerMenuManager;
        playerController = FindObjectOfType<PlayerCOntroller>();

    }

    void Update()
    {

        if (blockTime <= 0)
        {
            canBlock = false;
            blockTime = 0;
        }
        if (blockTime >= 100)
        {
            canBlock = true;
            blockTime = 100;
        }

        if (comboTime >= 0)
        {
            comboTime -= 1 * 100 * Time.deltaTime;
        }

        if (!playerMenuManager.playerInWindow)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (comboTime <= 0)
                {
                    comboStreek = 1;
                    animator.SetBool("atack", true);
                    animator.SetBool("atack2", false);
                    animator.SetBool("atack3", false);

                    animator.SetBool("idle", false);

                    comboTime = 100;

                }
                else if (comboTime > 0 & comboStreek == 1)
                {
                    comboStreek = 2;
                    animator.SetBool("atack", false);
                    animator.SetBool("atack2", true);
                    animator.SetBool("atack3", false);

                    animator.SetBool("idle", false);

                    comboTime = 100;
                }
                else if (comboTime > 0 & comboStreek == 2)
                {
                    comboStreek = 0;
                    animator.SetBool("atack", false);
                    animator.SetBool("atack2", false);
                    animator.SetBool("atack3", true);

                    animator.SetBool("idle", false);

                    comboTime = 0;
                }



            }
            else
            {
                animator.SetBool("atack", false);
                animator.SetBool("atack2", false);
                animator.SetBool("idle", true);
            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                blockTime -= 1 * 100 * Time.deltaTime;
                if (canBlock)
                {
                    animator.SetBool("block", true);
                    playerController.canTakeDamage = false;
                    
                }
                else
                {
                    animator.SetBool("block", false);
                    playerController.canTakeDamage = true;
                    
                }

                
            }
            else
            {
                animator.SetBool("block", false);
                playerController.canTakeDamage = true;
                blockTime += 1 * 30 * Time.deltaTime;

            }
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                animator.SetBool("block", false);
                playerController.canTakeDamage = true;
                blockTime += 1 * 5 * Time.deltaTime;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                animator.SetBool("atack", false);
                animator.SetBool("atack2", false);
                animator.SetBool("idle", true);
            }
        }
    }
}