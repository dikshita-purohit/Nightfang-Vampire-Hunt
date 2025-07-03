
using UnityEngine;

public class Fire_attack : MonoBehaviour
{
    public float lifeTime = 10f; 
    public int damage = 10; 

    void Start()
    {

        Destroy(gameObject, lifeTime);
    }

   void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Wall"))
        {
            EnemyScript enemy = other.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }

}
