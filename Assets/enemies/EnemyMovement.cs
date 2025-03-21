using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Rigidbody2D enemyRb;
    public Animator animator;

    public float waitTime = 5f;
    public float minMoveDistance = 1f;
    public float maxMoveDistance = 4f;
    public int damagePerHit = 1;
    public float chaseDistance = 4f;
    public float stopChaseDistance = 1f;
    public float escapeDistance = 5f;
    public float attackCooldown = 2f;
    public float attackDelay = 0f;
    private GameObject player;
    private Vector2 movement;
    private bool isChasing = false;
    private bool isAttacking = false;
    private bool isCollidingWithPlayer = false;
    private Vector2 lastMoveDirection = Vector2.down;
    private EnemyValues enemyValues;

    void Start()
    {
        enemyRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        enemyRb.mass = 20f;
        enemyRb.linearDamping = 10f;

        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("No se encontró un objeto con la etiqueta 'Player' en la escena.");
        }

        enemyValues = GetComponent<EnemyValues>();

    }

    void Update()
    {
        if (enemyValues != null && enemyValues.IsDead())
        {
            return;
        }

        float distanceFromPlayer = Vector2.Distance(player.transform.position, transform.position);

        if (!isAttacking)
        {
            if (distanceFromPlayer < chaseDistance && distanceFromPlayer > stopChaseDistance)
            {
                if (!isChasing)
                {
                    StopAllCoroutines();
                    isChasing = true;
                    enemyRb.linearVelocity = Vector2.zero;
                    StartCoroutine(ChasePlayer());
                }
            }
            else if (distanceFromPlayer > escapeDistance && isChasing)
            {
                isChasing = false;
                StopAllCoroutines();
                enemyRb.linearVelocity = Vector2.zero;
                StartCoroutine(RestartPatrol());
            }
        }

        if (!isChasing && !isAttacking)
        {
            animator.SetFloat("Horizontal", lastMoveDirection.x);
            animator.SetFloat("Vertical", lastMoveDirection.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }

        // Limitar la velocidad máxima si es empujado
        float maxPushSpeed = 2f;
        if (enemyRb.linearVelocity.magnitude > maxPushSpeed)
        {
            enemyRb.linearVelocity = enemyRb.linearVelocity.normalized * maxPushSpeed;
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
        enemyRb.linearVelocity = Vector2.zero;
    }

    IEnumerator MoveRandomly()
    {
        while (!isChasing && !isAttacking)
        {
            ChooseRandomDirection();
            yield return new WaitForSeconds(waitTime);
        }
    }

    void ChooseRandomDirection()
    {
        int randomDirection = Random.Range(0, 4);
        float randomDistance = Random.Range(minMoveDistance, maxMoveDistance);

        switch (randomDirection)
        {
            case 0: movement = Vector2.left; break;
            case 1: movement = Vector2.right; break;
            case 2: movement = Vector2.up; break;
            case 3: movement = Vector2.down; break;
        }
        lastMoveDirection = movement;
        Vector2 targetPosition = enemyRb.position + (movement * randomDistance);

        if (IsPositionValid(targetPosition))
        {
            StartCoroutine(MoveToPosition(targetPosition));
        }
        else
        {
            ChooseRandomDirection();
        }
    }

    private bool IsPositionValid(Vector2 targetPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(enemyRb.position, (targetPosition - enemyRb.position).normalized, Vector2.Distance(enemyRb.position, targetPosition));

        if (hit.collider == null)
        {
            return true;
        }

        if (hit.collider.gameObject != player && !hit.collider.gameObject.CompareTag("Enemy"))
        {
            return false;
        }

        return true;
    }
    IEnumerator MoveToPosition(Vector2 targetPosition)
    {
        while (!isChasing && !isAttacking && (targetPosition - enemyRb.position).sqrMagnitude > 0.01f)
        {
            if (!IsPositionValid(targetPosition))
            {
                StopAllCoroutines();
                enemyRb.linearVelocity = Vector2.zero;
                movement = Vector2.zero;
                StartCoroutine(RestartPatrol());
                yield break;
            }

            enemyRb.MovePosition(Vector2.MoveTowards(enemyRb.position, targetPosition, moveSpeed * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }

        enemyRb.position = targetPosition;
        movement = Vector2.zero;
    }

    IEnumerator ChasePlayer()
    {
        while (isChasing && !isAttacking)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            enemyRb.linearVelocity = direction * moveSpeed;

            lastMoveDirection = direction;

            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
            animator.SetFloat("Speed", direction.sqrMagnitude);

            yield return null;
        }

        enemyRb.linearVelocity = Vector2.zero;
    }

    IEnumerator RestartPatrol()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(MoveRandomly());
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        enemyRb.linearVelocity = Vector2.zero;
        enemyRb.bodyType = RigidbodyType2D.Kinematic; // Evitar que lo empujen durante el ataque

        Vector2 direction = (player.transform.position - transform.position).normalized;
        animator.SetFloat("Horizontal", lastMoveDirection.x);
        animator.SetFloat("Vertical", lastMoveDirection.y);
        animator.SetFloat("Speed", 0);
        animator.SetBool("IsAttacking", true);

        yield return new WaitForSeconds(attackDelay);
        PlayerValues playerValues = player.GetComponent<PlayerValues>();

        while (isCollidingWithPlayer)
        {
            if (playerValues != null)
            {
                playerValues.TakeDamage(damagePerHit);
            }
            yield return new WaitForSeconds(attackCooldown);
        }

        animator.SetBool("IsAttacking", false);
        isAttacking = false;
        enemyRb.bodyType = RigidbodyType2D.Dynamic; // Volver a la normalidad

        // Si el jugador sigue cerca, sigue persiguiendo
        float distanceFromPlayer = Vector2.Distance(player.transform.position, transform.position);
        if (distanceFromPlayer < chaseDistance)
        {
            isChasing = true;
            StartCoroutine(ChasePlayer());
        }
        else
        {
            StartCoroutine(RestartPatrol());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            isCollidingWithPlayer = true; // Marcar que hay colisión con el jugador

            if (!isAttacking) // Iniciar ataque solo si no está atacando ya
            {
                StartCoroutine(AttackPlayer());
            }
        }
        else if (!collision.gameObject.CompareTag("Enemy"))
        {
            StopAllCoroutines();
            enemyRb.linearVelocity = Vector2.zero;
            movement = Vector2.zero;

            if (!isChasing && !isAttacking)
            {
                StartCoroutine(RestartPatrol());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            isCollidingWithPlayer = false; // Marcar que el jugador ya no está en colisión
        }
    }

    public void StartPatrol()
    {
        if (!isChasing && !isAttacking)
        {
            StartCoroutine(MoveRandomly());
        }
    }
}
