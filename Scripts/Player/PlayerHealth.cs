using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;
    private Animator animator;
    private bool isDead = false;
    [Header("Audio")]
    public AudioClip gameOverSound;
    private AudioSource audioSource;

    [Header("Optional UI")]
    public PlayerHealthbar healthBar; 

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        if (animator != null)
            animator.SetTrigger("Die");

        var controller = GetComponent<PlayerMovement>();
        if (controller != null)
            controller.enabled = false;

        var collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = false;

        audioSource.PlayOneShot(gameOverSound);
        SceneManager.LoadScene("GameOver");
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);
    }
}
