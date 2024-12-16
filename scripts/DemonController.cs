using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DemonController : MonoBehaviour
{
    public Transform player; // Player's Transform
    public Transform[] waypoints; // Patrol waypoints
    private NavMeshAgent agent;

    public Slider healthSlider; // UI for health
    public Transform healthBarTransform; // To face the camera

    public Animator animator;
    public sorcererabilities sorcerer;
    private float maxHealth = 40f;
    public float currentHealth;
    levels level;

    private int currentWaypointIndex = 0;

    public bool isPatrolling = true;
    public bool isRunning = false;
    public bool isAttacking = false;
    private bool isDead = false;

    public float attackRange = 3f;
    private float attackCooldown = 2f;
    private float attackTimer = 0f;
    private int attackStep = 0; // To manage attack sequence


    void Start()


    {

        level = sorcerer.GetComponent<levels>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = 0.9f;
        if (!agent.isOnNavMesh)
        {
            Debug.LogError($"{gameObject.name} is not placed on a valid NavMesh!");
        }
        animator = GetComponent<Animator>();

        if (animator.runtimeAnimatorController == null)
        {
            Debug.LogError("AnimatorController is not assigned!");
        }
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    void Update()
    {
        if (isDead) return;

        if (isPatrolling)
        {
            Patrol();
        }
        else if (isRunning)
        {

            Run();
        }
        else if (isAttacking)
        {
            AttackPlayer();
        }



    }

    private void LateUpdate()
    {
        healthBarTransform.LookAt(Camera.main.transform.position);
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        agent.SetDestination(waypoints[currentWaypointIndex].position);
        // animator.SetBool("isWalking", true);

        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
    void Run()
    {
        agent.SetDestination(player.position);
        // Debug.Log("egryyyyy");
        // Stop moving

        animator.SetBool("walk", false);


    }
    void AttackPlayer()
    {
        //agent.SetDestination(transform.position); // Stop moving
        //animator.SetBool("runtocharacter", true);
        //Debug.Log(attackStep);
        // Debug.Log("attacckkkkk");

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            switch (attackStep)
            {
                case 0: // First sword swing

                    animator.SetInteger("state", 1);
                    attackTimer = attackCooldown;
                    attackStep++;
                    break;

                case 1: // Wait
                    animator.SetInteger("state", 2);
                    //animator.SetBool("runtocharacter", false);

                    attackTimer = attackCooldown;
                    attackStep++;
                    break;

                case 2: // Second sword swing

                    animator.SetInteger("state", 3);
                    // animator.SetBool("runtocharacter", false);
                    attackTimer = attackCooldown;
                    attackStep++;
                    break;

                case 3:

                    animator.SetInteger("state", 4);
                    attackTimer = attackCooldown;
                    attackStep = 0; // Reset attack sequence
                    break;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
        Debug.Log("Healthyyyy: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);

        // Notify the campentry script to remove this demon
        var camp = FindObjectOfType<campenterr>();
        if (camp != null)
        {
            camp.RemoveDemon(this); // Custom method to handle removal
        }

        Destroy(gameObject, 2f); // Remove demon after death animation plays
        level.increaseXp(30);
    }


    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("collisionnnn");

        if (other.CompareTag("Player"))
        {
            isPatrolling = false;
            isRunning = false;
            isAttacking = true;
        }
    }
    public void damagePlayer()
    {
        sorcerer.newhealth -= 10;
        if (sorcerer.newhealth <= 0)
        {
            sorcerer.newhealth = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("outtttt");
            isPatrolling = false;
            isRunning = true;
            isAttacking = false;
            attackStep = 0;
            animator.SetInteger("state", 0);

        }
    }
}
