using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CambioNivel : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;
    void Start()
    {

    }

    void Update()
    {

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("cambio"))
        {
            CargarSigNivel();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("cambio"))
        {
            CargarSigNivel();
        }
    }

    public void CargarSigNivel()
    {
        StartCoroutine(CargarNivel(SceneManager.GetActiveScene().buildIndex + 1));
    }


    IEnumerator CargarNivel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
