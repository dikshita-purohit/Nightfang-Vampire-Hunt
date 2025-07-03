using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float detectionRange = 8f;
    public float attackRange = 3f;
    public float moveSpeed = 2f;
    public float attackCooldown = 1f;
    public int attackDamage = 10;

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;
    public EnemyHealthBar healthBar;

    [Header("Attack Effects")]
    public GameObject fireEffectPrefab;
    public Transform fireEffectSpawnPoint;
    public float projectileSpeed = 10f;
    public float spawnOffsetDistance = 0.5f;

    private GameObject player;
    private bool isAttacking = false;
    private bool isDead = false;

    private bool isChasing = false;

    private Animator animator;
    private Vector2 lastMoveDir = Vector2.down;
    
    [Header("Audio Settings")]
    public AudioClip fireShootSound;
    public AudioClip deathSound; 
    private AudioSource audioSource;


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }
    }

    private void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;

            if (distanceToPlayer > attackRange)
            {
                if (!isAttacking)
                    MoveTowards(player.transform.position);
            }
            else
            {
                if (!isAttacking)
                {
                    StopMoving();
                    AttackPlayer();
                }
            }
        }
        else
        {
            isChasing = false;

            if (!isAttacking)
                Patrol();
        }
        
    }

    private void MoveTowards(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        if (direction != Vector2.zero)
            lastMoveDir = direction;

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
    }

    private void StopMoving()
    {
        animator.SetFloat("MoveX", lastMoveDir.x);
        animator.SetFloat("MoveY", lastMoveDir.y);
    }


    private void Patrol()
    {
        if (isChasing || patrolPoints == null || patrolPoints.Length == 0)
            return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];

        MoveTowards(targetPoint.position);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPatrolIndex++;

            if (currentPatrolIndex >= patrolPoints.Length)
                currentPatrolIndex = 0;
        }
    }

    private void AttackPlayer()
    {
        isAttacking = true;

        Vector2 direction = (player.transform.position - transform.position).normalized;

        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
        animator.SetTrigger("Attack");

        PlayFireEffect();

        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    private void PlayFireEffect()
    {
        if (fireEffectPrefab == null)
            return;

        Vector2 direction = new Vector2(
            animator.GetFloat("MoveX"),
            animator.GetFloat("MoveY")
        ).normalized;

        if (direction == Vector2.zero)
            direction = Vector2.down;

        Vector3 spawnPosition = fireEffectSpawnPoint.position;

        GameObject fire = Instantiate(fireEffectPrefab, spawnPosition, Quaternion.identity);

        if (fire != null)
        {
            Rigidbody2D rb = fire.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * projectileSpeed;
            }

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            fire.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        if (audioSource != null && fireShootSound != null)
        {
            Debug.Log("Playing fire shoot sound");
            audioSource.PlayOneShot(fireShootSound);
        }
        else
        {
            Debug.LogWarning("Missing audioSource or fireShootSound");
        }
    }



    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("Die");

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        Destroy(gameObject, 2f); 
    }

}
