using UnityEngine;

public class PersistenciaCamara : MonoBehaviour
{
    private static PersistenciaCamara instancia;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Evita duplicados si la cámara ya existe
        }
    }
}
