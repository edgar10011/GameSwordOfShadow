using UnityEngine;
using UnityEngine.SceneManagement;

public class DesactivarEnCinematica : MonoBehaviour
{
    public GameObject[] objetosADesactivar;
    public string nombreEscenaCinematica = "Cinematica";
    public AudioSource musica; // Arrastra aquí tu AudioSource con la música

    void Start()
    {
        // Verifica si estamos en la escena de la cinemática
        if (SceneManager.GetActiveScene().name == nombreEscenaCinematica)
        {
            // Desactiva los objetos
            foreach (GameObject obj in objetosADesactivar)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }

            // Detiene la música
            if (musica != null)
            {
                musica.Stop();
            }
        }
    }
}
