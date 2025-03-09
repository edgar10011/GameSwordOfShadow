using UnityEngine;

public class GameOverCanvasBehavior : MonoBehaviour
{
    private void Awake()
    {
        // Asegura que el Canvas no se destruya al cargar una nueva escena
        DontDestroyOnLoad(gameObject);
    }
}
