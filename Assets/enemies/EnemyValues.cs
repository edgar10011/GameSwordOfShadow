using UnityEngine;
using System.Collections;

public class EnemyValues : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetBool("IsDead", true);

        float deathAnimationDuration = animator.GetCurrentAnimatorStateInfo(0).length;

        Destroy(gameObject, 1f);
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = new Color(1f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
