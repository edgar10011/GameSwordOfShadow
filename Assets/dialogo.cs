using System.Collections;
using UnityEngine;

public class dialogo : MonoBehaviour
{
    private bool isPlayerInRange; // Corregido de "boll" a "bool"

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Se puede iniciar un dialogo"); // Agregado el ;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("No se puede iniciar un dialogo"); // Agregado el ;
        }
    }
}
