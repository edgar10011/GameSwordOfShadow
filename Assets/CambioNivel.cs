using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CambioNivel : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    private string spawnTagDestino;

    private void Start()
    {
        // Reinicia la llave al iniciar el nivel
        PlayerPrefs.SetInt("TieneLlave", 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detección de la llave
        if (collision.gameObject.CompareTag("llave"))
        {
            PlayerPrefs.SetInt("TieneLlave", 1);  // Guarda que la llave fue recogida
            Destroy(collision.gameObject);        // Elimina la llave
            Debug.Log("Llave recogida");
        }

        // Validación para nivel 5
        if (collision.gameObject.CompareTag("nivel5"))
        {
            if (PlayerPrefs.GetInt("TieneLlave", 0) == 1)  
            {
                spawnTagDestino = "SpawnNivel5";
                StartCoroutine(CargarNivel(4)); 
            }
            else
            {
                Debug.Log("Necesitas la llave para pasar al nivel 5.");
                return;  // Bloquea el acceso si no tiene la llave
            }
        }
        else if (collision.gameObject.CompareTag("nivel8"))
        {
            spawnTagDestino = "SpawnFinal";
            StartCoroutine(CargarNivel(7)); 
        }
        else if (collision.gameObject.CompareTag("nivel7"))
        {
            spawnTagDestino = "SpawnNivel7";
            StartCoroutine(CargarNivel(6)); 
        }
        else if (collision.gameObject.CompareTag("nivel6"))
        {
            spawnTagDestino = "SpawnNivel6";
            StartCoroutine(CargarNivel(5)); 
        }
        else if (collision.gameObject.CompareTag("nivel3"))
        {
            spawnTagDestino = "SpawnNivel3";
            StartCoroutine(CargarNivel(2)); 
        }
        else if (collision.gameObject.CompareTag("nivel3.4"))
        {
            spawnTagDestino = "SpawnNivel3.4";
            StartCoroutine(CargarNivel(2)); 
        }
        else if (collision.gameObject.CompareTag("nivel3.5"))
        {
            spawnTagDestino = "SpawnNivel3.5";
            StartCoroutine(CargarNivel(2)); 
        }
        else if (collision.gameObject.CompareTag("nivel2"))
        {
            spawnTagDestino = "SpawnNivel2";
            StartCoroutine(CargarNivel(1));
        }
        else if (collision.gameObject.CompareTag("nivel2.2"))
        {
            spawnTagDestino = "SpawnNivel2.2";
            StartCoroutine(CargarNivel(1));
        }
        else if (collision.gameObject.CompareTag("nivel1"))
        {
            spawnTagDestino = "SpawnNivel1";
            StartCoroutine(CargarNivel(0));
        }
        else if (collision.gameObject.CompareTag("nivel4"))
        {
            spawnTagDestino = "SpawnNivel4";
            StartCoroutine(CargarNivel(3)); 
        }
        else if (collision.gameObject.CompareTag("nivel5.6"))
        {
            spawnTagDestino = "SpawnNivel5.6";
            StartCoroutine(CargarNivel(4)); 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Detección de la llave
        if (other.CompareTag("llave"))
        {
            PlayerPrefs.SetInt("TieneLlave", 1);  
            Destroy(other.gameObject);           
            Debug.Log("Llave recogida");
        }

        // Validación para nivel 5
        if (other.CompareTag("nivel5"))
        {
            if (PlayerPrefs.GetInt("TieneLlave", 0) == 1)  
            {
                spawnTagDestino = "SpawnNivel5";
                StartCoroutine(CargarNivel(4));  
            }
            else
            {
                Debug.Log("Necesitas la llave para pasar al nivel 5.");
                return;  // Bloquea el acceso si no tiene la llave
            }
        }
        else if (other.CompareTag("nivel8"))
        {
            spawnTagDestino = "SpawnFinal";
            StartCoroutine(CargarNivel(7)); 
        }
        else if (other.CompareTag("nivel7"))
        {
            spawnTagDestino = "SpawnNivel7";
            StartCoroutine(CargarNivel(6)); 
        }
        else if (other.CompareTag("nivel5.6"))
        {
            spawnTagDestino = "SpawnNivel5.6";
            StartCoroutine(CargarNivel(4)); 
        }
        else if (other.CompareTag("nivel4"))
        {
            spawnTagDestino = "SpawnNivel4";
            StartCoroutine(CargarNivel(3)); 
        }
        else if (other.CompareTag("nivel3"))
        {
            spawnTagDestino = "SpawnNivel3";
            StartCoroutine(CargarNivel(2)); 
        }
        else if (other.CompareTag("nivel3.4"))
        {
            spawnTagDestino = "SpawnNivel3.4";
            StartCoroutine(CargarNivel(2)); 
        }
        else if (other.CompareTag("nivel2"))
        {
            spawnTagDestino = "SpawnNivel2";
            StartCoroutine(CargarNivel(1));
        }
        else if (other.CompareTag("nivel2.2"))
        {
            spawnTagDestino = "SpawnNivel2.2";
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
                float x = PlayerPrefs.GetFloat("JugadorX", 0);
                float y = PlayerPrefs.GetFloat("JugadorY", 0);
                jugador.transform.position = new Vector3(x, y, 0);
            }
        }
    }
}
