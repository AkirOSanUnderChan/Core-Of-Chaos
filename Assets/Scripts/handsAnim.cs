using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handsAnim : MonoBehaviour
{
    private PlayerCOntroller playerController;


    private playerMenuManager playerMenuManager;
    public Animator animator;


    private bool isFirstAttack = true;
    private float lastAttackTime = 0f;
    private float comboDelay = 0.1f;


    public float blockTime = 100;
    public float blockDelay;
    public bool canBlock;

    // Start is called before the first frame update
    void Start()
    {


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



        if (!playerMenuManager.playerInWindow)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                float currentTime = Time.time;

                if (isFirstAttack || (currentTime - lastAttackTime > comboDelay))
                {
                    if (!isFirstAttack && currentTime - lastAttackTime <= comboDelay)
                    {
                        // якщо не перша атака ≥ м≥ж атаками менше comboDelay секунд, в≥дтворити ан≥мац≥ю atack2
                        animator.SetBool("atack", false);
                        animator.SetBool("atack2", true);
                        isFirstAttack = true;
                    }
                    else
                    {
                        // ≤накше в≥дтворити ан≥мац≥ю atack
                        animator.SetBool("atack", true);
                        animator.SetBool("atack2", false);
                        isFirstAttack = false;
                    }

                    animator.SetBool("idle", false);
                    lastAttackTime = currentTime;
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