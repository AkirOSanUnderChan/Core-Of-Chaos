using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;


public class SkeletonEnemyController : MonoBehaviour
{
    private PlayerCOntroller playerController;

    [SerializeField] private Transform target;

    public string EnemyName;

    public int XPForKillEnemy;
    public Item coinsTypeForKillEnemy;
    public int CointForKillEnemy;
    public TextMeshProUGUI enemyhpCount;
    public Slider enemySliderHP;
    public TextMeshProUGUI EnemyNameText;

    public AudioSource EnemyDamageSourse;
    public AudioClip TakeDamageSound;
    public AudioClip DeathSound;

    public int enemyDamage;
    public int enemyMaxHP;
    public int enemyCurrentHP;

    public Item enemyDrop;
    public int enemyDropAmount;
    public float enemyDropChance;

    Animator animator;
    private Transform player;
    private Transform closestBasePosition;
    private NavMeshAgent agent;
    public float distanceToPlayer;
    public float maxChaseDistance = 20f;

    public bool IS_DEAD;

    CapsuleCollider enemyColider;
    BoxCollider enemySwordColider;

    // Start is called before the first frame update
    void Start()
    {
        IS_DEAD = false;
        enemyColider = GetComponent<CapsuleCollider>();
        enemySwordColider = GetComponentInChildren<BoxCollider>();
        enemyCurrentHP = enemyMaxHP;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = PlayerControllerSingleton.Instance.PlayerCOntroller;

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            Debug.LogWarning("Об'єкту з тегом 'Player' не знайдено!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IS_DEAD)
        {
            enemySwordColider.enabled = false;
            enemyColider.enabled = false;
            animator.SetBool("walk", false);
            animator.SetBool("idle", false);
            animator.SetBool("atack", false);
        }
        enemyhpCount.SetText(enemyCurrentHP.ToString() + " / " + enemyMaxHP.ToString());
        enemySliderHP.maxValue = enemyMaxHP;
        enemySliderHP.value = enemyCurrentHP;
        EnemyNameText.SetText(EnemyName);



        target.transform.rotation = Camera.main.transform.rotation;

        if (player != null)
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Якщо playerController ще не має посилання, отримаємо його з компонента PlayerController гравця
            if (playerController == null)
            {
                playerController = player.GetComponent<PlayerCOntroller>();
            }
        }
        if (!IS_DEAD)
        {
            if (distanceToPlayer < maxChaseDistance) // Преслідування гравця
            {
                agent.stoppingDistance = 1.5f;
                animator.SetBool("walk", true);
                animator.SetBool("idle", false);
                animator.SetBool("atack", false);
                animator.SetBool("death", false);


                if (distanceToPlayer <= 2)
                {
                    animator.SetBool("walk", false);
                    animator.SetBool("idle", false);
                    animator.SetBool("atack", true);
                    animator.SetBool("death", false);

                }
                else
                {
                    agent.SetDestination(player.position);
                }
            }
            else // Ворог занадто далеко від гравця, йде до найближчої базової точки
            {
                closestBasePosition = FindClosestBasePosition();

                if (closestBasePosition != null)
                {
                    agent.SetDestination(closestBasePosition.position);
                    if (Vector3.Distance(transform.position, closestBasePosition.position) <= 9f)
                    {
                        agent.stoppingDistance = 5;
                        // Ворог досяг найближчої базової точки, почати анімацію бездіяльності
                        animator.SetBool("walk", false);
                        animator.SetBool("idle", true);
                        animator.SetBool("atack", false);
                        animator.SetBool("death", false);

                    }
                    else
                    {
                        animator.SetBool("walk", true);
                        animator.SetBool("idle", false);
                        animator.SetBool("atack", false);
                        animator.SetBool("death", false);

                    }
                }
                else
                {
                    Debug.LogWarning("Об'єкт з тегом 'Base' не знайдено!");
                }
            }
        }
        


    }

    // Метод для знаходження найближчої базової точки
    private Transform FindClosestBasePosition()
    {
        GameObject[] basePositions = GameObject.FindGameObjectsWithTag("Base");
        Transform closestPosition = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject basePos in basePositions)
        {
            float distance = Vector3.Distance(transform.position, basePos.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPosition = basePos.transform;
            }
        }

        return closestPosition;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerWeapon"))
        {
            EnemyDamageSourse.PlayOneShot(TakeDamageSound);
            if (playerController != null)
            {
                enemyCurrentHP -= playerController.playerDamage;

                if (enemyCurrentHP <= 0)
                {
                    if (!IS_DEAD)
                    {

                        IS_DEAD = true;
                        EnemyDamageSourse.PlayOneShot(DeathSound);

                        float randomValue = Random.Range(0, 100);
                        if (randomValue <= enemyDropChance)
                        {
                            InventoryManager.instance.AddItem(enemyDrop, enemyDropAmount);
                        }
 

                        animator.SetBool("death", true);
                        Debug.Log("Enemy is Died");

                        playerController.ClaimExperience(XPForKillEnemy);
                        InventoryManager.instance.AddItem(coinsTypeForKillEnemy, CointForKillEnemy);
                        Destroy(gameObject, 2);
                    }

                    

                }
            }
            else
            {
                Debug.LogWarning("PlayerController not found!");
            }
        }
    }



    public void giveDamage()
    {
        playerController.currentHP -= enemyDamage;
    }
}
