using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    public Button playButton; // Botón para iniciar el juego
    public Button settingsButton; // Botón para abrir configuraciones
    public Button exitButton; // Botón para salir del juego
    public Slider volumeSlider; // Slider de volumen

    private bool isSliderActive = false; // Verifica si el slider está activo

    void Start()
    {
        // Seleccionar el botón por defecto
        playButton.Select();
        EventSystem.current.SetSelectedGameObject(playButton.gameObject);

        // Asignar eventos a los botones
        playButton.onClick.AddListener(IniciarJuego);
        settingsButton.onClick.AddListener(AbrirConfiguraciones);
        exitButton.onClick.AddListener(SalirDelJuego);

        volumeSlider.gameObject.SetActive(false); // Esconder el slider al inicio
    }

    void Update()
    {
        // Si el jugador presiona Enter o el botón "Submit"
        if (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.Return))
        {
            // Detectar qué botón está seleccionado actualmente
            GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

            if (currentSelected == playButton.gameObject)
            {
                IniciarJuego();
            }
            else if (currentSelected == settingsButton.gameObject)
            {
                AbrirConfiguraciones();
            }
            else if (currentSelected == exitButton.gameObject)
            {
                SalirDelJuego();
            }
        }

        // Mover el foco de vuelta al slider si está activo
        if (isSliderActive && Input.GetKeyDown(KeyCode.Escape)) // Cerrar slider con 'Esc'
        {
            CerrarSlider();
        }
    }

    void IniciarJuego()
    {
        SceneManager.LoadScene("nivel1"); // Cambiar a la escena del juego
    }

    void AbrirConfiguraciones()
    {
        // Mostrar el slider de volumen y seleccionarlo
        isSliderActive = !volumeSlider.gameObject.activeSelf;
        volumeSlider.gameObject.SetActive(isSliderActive);

        if (isSliderActive)
        {
            EventSystem.current.SetSelectedGameObject(volumeSlider.gameObject); // Seleccionar el slider
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(settingsButton.gameObject); // Regresar al botón si se cierra el slider
        }

        Debug.Log("Abrir menú de configuraciones");
    }

    void CerrarSlider()
    {
        volumeSlider.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(settingsButton.gameObject); // Regresar al botón de configuración
        isSliderActive = false;
    }

    void SalirDelJuego()
    {
        Debug.Log("Salir del juego");
        Application.Quit();
    }
}
