using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusic : MonoBehaviour
{
    private static MenuMusic instance; // Para asegurarnos de que solo haya una instancia

    void Awake()
    {
        if (instance == null) // Si no existe una instancia, la creamos y la mantenemos
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Si ya hay una instancia, destruimos la nueva para evitar duplicados
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MenuPrincipal")
        {
            Destroy(gameObject); // Destruye la música cuando se cambie de escena
        }
    }
}
