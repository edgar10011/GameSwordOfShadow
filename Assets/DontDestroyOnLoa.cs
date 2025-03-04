using UnityEngine;

public class PersistenciaJugador : MonoBehaviour
{
    private static PersistenciaJugador instancia;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }
}
