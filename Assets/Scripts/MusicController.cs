using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public Button musicButton;   // Botón de música
    public Slider volumeSlider;  // Slider de volumen
    private AudioSource menuMusic; // AudioSource de la música

    void Start()
    {
        menuMusic = FindObjectOfType<AudioSource>(); // Encuentra el AudioSource en la escena

        // Si hay un volumen guardado, lo carga; si no, usa 1 (volumen máximo)
        float savedVolume = PlayerPrefs.HasKey("MusicVolume") ? PlayerPrefs.GetFloat("MusicVolume") : 1f;
        menuMusic.volume = savedVolume;
        volumeSlider.value = savedVolume;
        volumeSlider.gameObject.SetActive(false); // Ocultar slider al inicio

        // Asignar eventos
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
        musicButton.onClick.AddListener(ToggleSlider);
    }

    void ChangeVolume(float volume)
    {
        menuMusic.volume = volume; // Ajusta el volumen del AudioSource
        PlayerPrefs.SetFloat("MusicVolume", volume); // Guarda la preferencia
        PlayerPrefs.Save();
    }

    void ToggleSlider()
    {
        volumeSlider.gameObject.SetActive(!volumeSlider.gameObject.activeSelf); // Mostrar/Ocultar slider
    }
}
