using UnityEngine;
using UnityEngine.SceneManagement;

public class EliminarDirectamenteDontDestroyOnLoad : MonoBehaviour
{
    public string nombreEscenaCinematica = "Cinematica";

    void Start()
    {
        // Verifica si estamos en la escena de la cinemática
        if (SceneManager.GetActiveScene().name == nombreEscenaCinematica)
        {
            // Busca y elimina todos los objetos en DontDestroyOnLoad
            GameObject[] allObjects = FindObjectsOfType<GameObject>(true);
            foreach (GameObject obj in allObjects)
            {
                if (obj.scene.name == null || obj.scene.name == "DontDestroyOnLoad")
                {
                    Destroy(obj);
                    Debug.Log($"Objeto eliminado: {obj.name}");
                }
            }
        }
    }
}
