using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CambioNivel : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    private string spawnTagDestino;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("nivel2"))
        {
            spawnTagDestino = "SpawnNivel2";
            StartCoroutine(CargarNivel(1));
        }
        else if (collision.gameObject.CompareTag("nivel1"))
        {
            spawnTagDestino = "SpawnNivel1";
            StartCoroutine(CargarNivel(0));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("nivel2"))
        {
            spawnTagDestino = "SpawnNivel2";
            StartCoroutine(CargarNivel(1));
        }
        else if (other.CompareTag("nivel1"))
        {
            spawnTagDestino = "SpawnNivel1";
            StartCoroutine(CargarNivel(0));
        }
    }

    IEnumerator CargarNivel(int levelIndex)
    {
        if (transition != null)
        {
            transition.SetTrigger("Start");
        }

        // Guardar posición del jugador antes de cambiar de nivel
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            PlayerPrefs.SetFloat("JugadorX", jugador.transform.position.x);
            PlayerPrefs.SetFloat("JugadorY", jugador.transform.position.y);
            PlayerPrefs.SetString("SpawnTag", spawnTagDestino);
        }

        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            string savedSpawnTag = PlayerPrefs.GetString("SpawnTag", "");
            GameObject spawnPoint = GameObject.FindGameObjectWithTag(savedSpawnTag);

            if (spawnPoint != null)
            {
                jugador.transform.position = spawnPoint.transform.position;
            }
            else
            {
                // Si el spawn no se encuentra, usa la última posición guardada
                float x = PlayerPrefs.GetFloat("JugadorX", 0);
                float y = PlayerPrefs.GetFloat("JugadorY", 0);
                jugador.transform.position = new Vector3(x, y, 0);
            }
        }
    }
}
