using UnityEngine;

public class PlayerValues : MonoBehaviour
{
    public int maxHealth = 10;
    public int maxStamina = 10;
    public int currentHealth;
    public int currentStamina;
    public HealthBar healthBar;
    public StaminaBar staminaBar;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }

    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        audioManager.PlayRandomHurtSound();
        Debug.Log("Vida" + currentHealth);
    }

    public void LoseStamina(int stamina)
    {
        currentStamina -= stamina;
        staminaBar.SetStamina(currentStamina);
        //Si baja de 2 sonar el audio
    }
}
