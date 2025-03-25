using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseCanvas;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pauseCanvas.activeInHierarchy)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseCanvas.SetActive(true);
        Time.timeScale = 0f; // Pausa el juego
    }

    public void ResumeGame()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f; // Reanuda el juego
    }

    public void ExitGame()
{
    Time.timeScale = 1f; // Asegúrate de reanudar el tiempo antes de reiniciar
    SceneManager.LoadScene("MenuPrincipal"); // Carga el nivel 1 para reiniciar el juego
}


}
