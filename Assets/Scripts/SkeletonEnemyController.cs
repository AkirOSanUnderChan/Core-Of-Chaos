using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

public class SkeletonEnemyController : MonoBehaviour
{
    private PlayerCOntroller playerController;

    [SerializeField] private Transform target;

    public int XPForKillEnemy = 10;
    public TextMeshProUGUI enemyhpCount;
    public Slider enemySliderHP;

    public int enemyMaxHP = 5;
    public int enemyCurrentHP;

    Animator animator;
    private Transform player;
    private Transform closestBasePosition;
    private NavMeshAgent agent;
    public float distanceToPlayer;
    public float maxChaseDistance = 20f;

    // Start is called before the first frame update
    void Start()
    {
        enemyCurrentHP = enemyMaxHP;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = PlayerControllerSingleton.Instance.playerController;

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
        enemyhpCount.SetText(enemyCurrentHP.ToString() + " / " + enemyMaxHP.ToString());
        enemySliderHP.maxValue = enemyMaxHP;
        enemySliderHP.value = enemyCurrentHP;

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

        if (distanceToPlayer < maxChaseDistance) // Преслідування гравця
        {
            animator.SetBool("walk", true);
            animator.SetBool("idle", false);
            animator.SetBool("atack", false);

            if (distanceToPlayer <= 2)
            {
                animator.SetBool("walk", false);
                animator.SetBool("idle", false);
                animator.SetBool("atack", true);
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
                if (Vector3.Distance(transform.position, closestBasePosition.position) < 5f)
                {
                    // Ворог досяг найближчої базової точки, почати анімацію бездіяльності
                    animator.SetBool("walk", false);
                    animator.SetBool("idle", true);
                    animator.SetBool("atack", false);
                }
                else
                {
                    animator.SetBool("walk", true);
                    animator.SetBool("idle", false);
                    animator.SetBool("atack", false);
                }
            }
            else
            {
                Debug.LogWarning("Об'єкт з тегом 'Base' не знайдено!");
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
            if (playerController != null)
            {
                enemyCurrentHP -= playerController.playerDamage;

                if (enemyCurrentHP <= 0)
                {
                    Debug.Log("Enemy is Died");

                    playerController.playerCurrentXP += XPForKillEnemy;
                    playerController.playerBalance += 3;
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.LogWarning("PlayerController not found!");
            }
        }
    }
}
