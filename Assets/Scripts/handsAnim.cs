using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handsAnim : MonoBehaviour
{
    private playerMenuManager playerMenuManager;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMenuManager = PlayerControllerSingleton.Instance.playerMenuManager;

    }

    // Update is called once per frame
    void Update()
    {
        if (!playerMenuManager.playerInWindow)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("Ти не в вікні і ти зараз вдарив");
                animator.SetBool("atack", true);
                animator.SetBool("idle", false);
            }
            else
            {
                animator.SetBool("atack", false);
                animator.SetBool("idle", true);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("Ти в вікні і не можеш зараз вдарити");
                animator.SetBool("atack", false);
                animator.SetBool("idle", true);
            }
        }
    }
}
