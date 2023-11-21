using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handsAnim : MonoBehaviour
{
    private playerMenuManager playerMenuManager;
    public Animator animator;

    public int atackIndex;
    static bool isBlocked;
    // Start is called before the first frame update
    void Start()
    {
        atackIndex = 0;
        isBlocked = false;
        animator = GetComponent<Animator>();
        playerMenuManager = PlayerControllerSingleton.Instance.playerMenuManager;
        

    }

    // Update is called once per frame
    void Update()
    {
        if (!playerMenuManager.playerInWindow)
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                if (!isBlocked)
                {

                }
            }


            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("�� �� � ��� � �� ����� ������");
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
                Debug.Log("�� � ��� � �� ����� ����� �������");
                animator.SetBool("atack", false);
                animator.SetBool("idle", true);
            }
        }
    }
}
