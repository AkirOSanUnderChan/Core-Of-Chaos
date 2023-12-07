using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handsAnim : MonoBehaviour
{
    private playerMenuManager playerMenuManager;
    public Animator animator;


    private bool isFirstAttack = true;
    private float lastAttackTime = 0f;
    private float comboDelay = 0.1f;

    // Start is called before the first frame update
    void Start()
    {


        animator = GetComponent<Animator>();
        playerMenuManager = PlayerControllerSingleton.Instance.playerMenuManager;


    }

    void Update()
    {
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