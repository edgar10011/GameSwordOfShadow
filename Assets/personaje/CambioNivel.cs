using UnityEngine;
using UnityEngine.SceneManagement; // Importante para cambiar de escena

public class CambioNivel : MonoBehaviour
{
    void Start()
    {
        // Puedes inicializar variables si lo necesitas
    }

    void Update()
    {
        // No se necesita lógica en Update para cambiar de nivel
    }

    // Detecta colisiones normales (si el objeto tiene un Collider2D NO marcado como Trigger)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("cambio")) // Verifica el tag "cambio"
        {
            CambiarNivel();
        }
    }

    // Detecta colisiones con Triggers (si el Collider2D del objeto "cambio" tiene activado "Is Trigger")
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("cambio")) 
        {
            CambiarNivel();
        }
    }

    // Método para cambiar de nivel
    private void CambiarNivel()
    {
        Debug.Log("Cambiando de nivel...");
        int nivelActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(nivelActual + 1); // Carga la siguiente escena
    }
}
