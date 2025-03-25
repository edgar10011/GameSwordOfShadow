using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerValues : MonoBehaviour
{
    public int maxHealth = 10;
    public int maxStamina = 10;
    public int currentHealth;
    public int currentStamina;
    public HealthBar healthBar;
    public StaminaBar staminaBar;
    AudioManager audioManager;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;
    public GameObject gameOverCanvas;
    public Text gameOverText;
    private Animator playerAnimator;
    private CanvasGroup gameOverCanvasGroup;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
        playerAnimator = GetComponent<Animator>();
        gameOverCanvas.SetActive(true);
        gameOverCanvasGroup = gameOverCanvas.GetComponent<CanvasGroup>();
        if (gameOverCanvasGroup != null)
        {
            gameOverCanvasGroup.alpha = 0f;
            gameOverCanvasGroup.interactable = false;
            gameOverCanvasGroup.blocksRaycasts = false;
        }
    }

    void Update()
    {
        // Puedes añadir alguna lógica adicional si lo necesitas
    }

    public void TakeDamage(int damage)
    {
        if (isDead || playerAnimator.GetBool("IsProtecting")) return;

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        audioManager.PlayRandomHurtSound();
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
        Debug.Log("Jugador ha muerto");
        StartCoroutine(ShowGameOver());  // Mostrar Game Over por 3 segundos
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = new Color(1f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }

    public void LoseStamina(int stamina)
    {
        currentStamina -= stamina;
        staminaBar.SetStamina(currentStamina);
    }

    // Mostrar el GameOverCanvas y luego reaparecer al jugador
    private IEnumerator ShowGameOver()
    {
        var playerMovement = GetComponent<PlayerMovement>();
        playerMovement.enabled = false;
        audioManager.StopSFX();

        if (gameOverCanvasGroup != null)
        {
            gameOverCanvasGroup.interactable = true;
            gameOverCanvasGroup.blocksRaycasts = true;

            float fadeDuration = 1f;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                gameOverCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            gameOverCanvasGroup.alpha = 1f;
        }

        string message = "¡Has Perdido!";
        gameOverText.text = "";
        foreach (char c in message)
        {
            gameOverText.text += c;
            yield return new WaitForSeconds(0.1f);
        }

        // Esperar 3 segundos
        yield return new WaitForSeconds(3f);

        // Ocultar el Game Over canvas
        if (gameOverCanvasGroup != null)
        {
            gameOverCanvasGroup.alpha = 0f;
            gameOverCanvasGroup.interactable = false;
            gameOverCanvasGroup.blocksRaycasts = false;
        }

        // Reaparecer al jugador en la posición guardada del último cambio de nivel
        RespawnPlayer();
    }

    private void RespawnPlayer()
{
    GameObject jugador = GameObject.FindGameObjectWithTag("Player");
    if (jugador != null)
    {
        // COORDENADAS FIJAS DEFINIDAS DIRECTAMENTE EN EL CÓDIGO
        float respawnX = 5f;
        float respawnY = -10f;

        jugador.transform.position = new Vector3(respawnX, respawnY, 0);

        var playerMovement = jugador.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
        currentStamina = maxStamina;
        staminaBar.SetStamina(currentStamina);

        isDead = false;
        animator.SetBool("IsDead", false);
        animator.Rebind();
    }
}


}
