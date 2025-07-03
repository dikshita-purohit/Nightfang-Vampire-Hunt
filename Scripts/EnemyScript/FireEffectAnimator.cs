using UnityEngine;

public class FireEffectAnimator : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
            animator.CompareTag("Fire");

        Destroy(gameObject, 1f); 
    }
}
