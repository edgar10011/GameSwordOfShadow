using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float normalSpeed = 3f;
    [SerializeField] private float sprintSpeed = 5f;
    private float speed;
    private bool isSprinting = false;
    private bool canRegenerateStamina = true;
    private Coroutine staminaDrainCoroutine;
    AudioManager audioManager;
    public PlayerValues playerValues;

    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection = Vector2.down;
    private Animator playerAnimator;

    public StaminaBar staminaBar;
    private Coroutine regenStaminaCoroutine;

    private bool canHeal = true;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        speed = normalSpeed;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;
        if (moveInput.sqrMagnitude > 0)
        {
            lastMoveDirection = moveInput;
        }
        playerAnimator.SetFloat("Horizontal", moveX);
        playerAnimator.SetFloat("Vertical", moveY);
        playerAnimator.SetFloat("Speed", moveInput.sqrMagnitude);

        DetectArcadeButtons();
    }

    private void FixedUpdate()
    {
        if (moveInput.sqrMagnitude > 0)
        {
            playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);

            if (speed == normalSpeed)
            {
                if (audioManager.SFXSource.clip != audioManager.player_walk || !audioManager.SFXSource.isPlaying)
                {
                    audioManager.PlaySFXLoop(audioManager.player_walk);
                }
            }
            else if (speed == sprintSpeed)
            {
                if (audioManager.SFXSource.clip != audioManager.player_run || !audioManager.SFXSource.isPlaying)
                {
                    audioManager.PlaySFXLoop(audioManager.player_run);
                }
            }
        }
        else
        {
            audioManager.StopSFX();
        }
        if (moveInput.sqrMagnitude == 0)
        {
            playerRb.linearVelocity = Vector2.zero;
        }
    }

    private void DetectArcadeButtons()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            Debug.Log("Cubrirse");
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            StartSprint();
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton2))
        {
            StopSprint();
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton3) && canHeal)
        {
            StartCoroutine(HealOverTime(3, 3f));
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            Debug.Log("Golpe cargado");
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            Debug.Log("Esquivar");
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            Debug.Log("Cambio de arma");
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            Debug.Log("Interactuar con el entorno");
        }
    }

    private void StartSprint()
    {
        if (playerValues.currentStamina > 0)
        {
            isSprinting = true;
            speed = sprintSpeed;
            canRegenerateStamina = false;

            if (regenStaminaCoroutine != null)
            {
                StopCoroutine(regenStaminaCoroutine);
                regenStaminaCoroutine = null;
            }

            if (staminaDrainCoroutine == null)
            {
                staminaDrainCoroutine = StartCoroutine(DrainStamina());
            }


        }
    }

    private void StopSprint()
    {
        isSprinting = false;
        speed = normalSpeed;
        if (staminaDrainCoroutine != null)
        {
            StopCoroutine(staminaDrainCoroutine);
            staminaDrainCoroutine = null;
        }

        if (regenStaminaCoroutine != null)
        {
            StopCoroutine(regenStaminaCoroutine);
        }

        regenStaminaCoroutine = StartCoroutine(WaitAndRegenerateStamina());
    }

    private IEnumerator DrainStamina()
    {
        while (isSprinting && playerValues.currentStamina > 0)
        {
            playerValues.LoseStamina(1);
            yield return new WaitForSeconds(1f);
        }
        StopSprint();
    }

    private IEnumerator WaitAndRegenerateStamina()
    {
        yield return new WaitForSeconds(2f);
        canRegenerateStamina = true;
        StartCoroutine(RegenerateStamina());

        if (regenStaminaCoroutine != null)
        {
            StopCoroutine(regenStaminaCoroutine);
        }

        regenStaminaCoroutine = StartCoroutine(RegenerateStamina());
    }

    private IEnumerator RegenerateStamina()
    {
        while (canRegenerateStamina && playerValues.currentStamina < playerValues.maxStamina)
        {
            playerValues.currentStamina++;
            staminaBar.SetStamina(playerValues.currentStamina);
            yield return new WaitForSeconds(4f);
        }

        regenStaminaCoroutine = null;
    }

    private IEnumerator HealOverTime(int healAmount, float duration)
    {
        canHeal = false;
        int healed = 0;

        while (healed < healAmount && playerValues.currentHealth < playerValues.maxHealth)
        {
            playerValues.currentHealth++;
            playerValues.healthBar.SetHealth(playerValues.currentHealth);
            yield return new WaitForSeconds(duration / healAmount);
            healed++;
        }

        yield return new WaitForSeconds(5f);
        canHeal = true;
    }

    private void Attack()
    {
        if (!playerAnimator.GetBool("IsAttacking")) // Verifica que no esté atacando
        {
            playerAnimator.SetBool("IsAttacking", true);
            StartCoroutine(ResetAttackAnimation()); // Inicia la corrutina para resetear el ataque

            float attackRange = 1.0f;
            float attackWidth = 1f;

            // Dirección de ataque basada en la última dirección de movimiento
            Vector2 attackDirection = moveInput == Vector2.zero ? lastMoveDirection : moveInput;
            Vector2 attackOrigin = (Vector2)transform.position + attackDirection * 0.5f;

            // Dibujar el área de ataque para depuración
            Debug.DrawRay(attackOrigin, attackDirection * attackRange, Color.red, 0.5f);

            Vector2 topLeft = attackOrigin + new Vector2(-attackWidth / 2, attackRange / 2);
            Vector2 bottomRight = attackOrigin + new Vector2(attackWidth / 2, -attackRange / 2);

            Debug.DrawLine(topLeft, new Vector2(topLeft.x, bottomRight.y), Color.green, 0.5f);
            Debug.DrawLine(topLeft, new Vector2(bottomRight.x, topLeft.y), Color.green, 0.5f);
            Debug.DrawLine(bottomRight, new Vector2(topLeft.x, bottomRight.y), Color.green, 0.5f);
            Debug.DrawLine(bottomRight, new Vector2(bottomRight.x, topLeft.y), Color.green, 0.5f);

            // Buscar enemigos por su SpriteRenderer usando la nueva API
            EnemyValues[] enemies = Object.FindObjectsByType<EnemyValues>(FindObjectsSortMode.None);
            foreach (EnemyValues enemy in enemies)
            {
                if (IsEnemyInAttackArea(enemy, topLeft, bottomRight))
                {
                    enemy.TakeDamage(1);
                }
            }
        }
    }

    // Función para verificar si el SpriteRenderer del enemigo está dentro del área de ataque
    private bool IsEnemyInAttackArea(EnemyValues enemy, Vector2 topLeft, Vector2 bottomRight)
    {
        if (enemy == null) return false;

        SpriteRenderer enemySprite = enemy.GetComponent<SpriteRenderer>();
        if (enemySprite == null) return false;

        // Obtiene el área del sprite del enemigo
        Bounds enemyBounds = enemySprite.bounds;

        // Verifica si el centro del sprite está dentro del área de ataque
        return (enemyBounds.center.x >= topLeft.x && enemyBounds.center.x <= bottomRight.x) &&
               (enemyBounds.center.y >= bottomRight.y && enemyBounds.center.y <= topLeft.y);
    }



    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);
        playerAnimator.SetBool("IsAttacking", false);
    }


}