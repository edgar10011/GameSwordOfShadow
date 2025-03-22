using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    public Button playButton;      // Botón para iniciar el juego
    public Button settingsButton;  // Botón para abrir configuraciones
    public Button exitButton;      // Botón para salir del juego
    public Slider volumeSlider;    // Slider de volumen

    private bool isSliderActive = false;  // Verifica si el slider está activo

    void Start()
    {
        // Verifica si los botones están asignados antes de usarlos
        if (playButton != null)
        {
            playButton.Select();
            EventSystem.current.SetSelectedGameObject(playButton.gameObject);
            playButton.onClick.AddListener(IniciarJuego);
        }

        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(AbrirConfiguraciones);
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(SalirDelJuego);
        }

        // Verifica si el slider está asignado antes de acceder a él
        if (volumeSlider != null)
        {
            volumeSlider.gameObject.SetActive(false);  // Esconder el slider al inicio
        }
    }

    void Update()
    {
        // Verifica si EventSystem está presente para evitar errores
        if (EventSystem.current != null)
        {
            if (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.Return))
            {
                GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

                if (currentSelected == playButton?.gameObject)
                {
                    IniciarJuego();
                }
                else if (currentSelected == settingsButton?.gameObject)
                {
                    AbrirConfiguraciones();
                }
                else if (currentSelected == exitButton?.gameObject)
                {
                    SalirDelJuego();
                }
            }
        }

        // Cerrar el slider con 'Esc'
        if (isSliderActive && Input.GetKeyDown(KeyCode.Escape))
        {
            CerrarSlider();
        }
    }

    void IniciarJuego()
    {
        SceneManager.LoadScene("nivel1");
    }

    void AbrirConfiguraciones()
    {
        if (volumeSlider != null)
        {
            isSliderActive = !volumeSlider.gameObject.activeSelf;
            volumeSlider.gameObject.SetActive(isSliderActive);

            if (isSliderActive)
            {
                EventSystem.current.SetSelectedGameObject(volumeSlider.gameObject);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(settingsButton?.gameObject);
            }

            Debug.Log("Abrir menú de configuraciones");
        }
        else
        {
            Debug.LogWarning("Slider no asignado en el inspector.");
        }
    }

    void CerrarSlider()
    {
        if (volumeSlider != null)
        {
            volumeSlider.gameObject.SetActive(false);
        }
        EventSystem.current.SetSelectedGameObject(settingsButton?.gameObject);
        isSliderActive = false;
    }

    void SalirDelJuego()
    {
        Debug.Log("Salir del juego");
        Application.Quit();
    }
}
