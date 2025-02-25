using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1f;  // Ajusta esta velocidad desde el Inspector

    private Rigidbody2D playerRb;
    private Vector2 moveInput;

    private Animator playerAnimator;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Recibir entradas de movimiento
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        // Animaciones de movimiento
        playerAnimator.SetFloat("Horizontal", moveX);
        playerAnimator.SetFloat("Vertical", moveY);
        playerAnimator.SetFloat("Speed", moveInput.sqrMagnitude);
    }

    // FixedUpdate se usa para la física (movimiento del personaje)
    private void FixedUpdate()
    {
        // Verificar la velocidad en la consola para depuración
        Debug.Log("Speed: " + speed);

        // Ajustar la velocidad y mover al personaje
        playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);
        
        // O usar velocity (opcional):
        // playerRb.velocity = moveInput * speed;
    }
}
