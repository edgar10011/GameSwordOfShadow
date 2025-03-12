using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    public Button exitButton;

    void Start()
    {
        exitButton.onClick.AddListener(CloseGame);
    }

    void CloseGame()
    {
        Debug.Log("Cerrando el juego..."); // Mensaje en la consola
        Application.Quit(); // Cierra la aplicación

        // Si estamos en el editor de Unity, detener la ejecución
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
