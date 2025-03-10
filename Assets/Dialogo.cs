using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogo : MonoBehaviour
{
    [SerializeField] private GameObject Exclamation_Gray;
    [SerializeField] private GameObject DialoguePanel;
    [SerializeField] private TMP_Text DialogueText;
    [SerializeField, TextArea(4,6)] private string[] dialogueLines;

    private float typingTime = 0.05f;
    private bool isPlayerInRange; 
    private bool didDialogueStart;
    private int lineIndex;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Tecla X presionada dentro del área de diálogo");
            if (!didDialogueStart)
            {
                StartDialogue();
            }
            else
            {
                NextLine();
            }
        }
    }

    private void StartDialogue()
    {
        if (dialogueLines.Length == 0)
        {
            Debug.LogError("No hay diálogos en el array.");
            return;
        }
        
        didDialogueStart = true;
        DialoguePanel.SetActive(true);
        Debug.Log("Panel de diálogo activado");
        Exclamation_Gray.SetActive(false);
        lineIndex = 0;
        StartCoroutine(ShowLine());
    }

    private IEnumerator ShowLine()
    {
        if (lineIndex >= dialogueLines.Length)
        {
            Debug.LogError("Intentando acceder a una línea fuera de rango.");
            yield break;
        }

        DialogueText.text = string.Empty;
        Debug.Log("Mostrando texto: " + dialogueLines[lineIndex]);
        
        foreach (char ch in dialogueLines[lineIndex])
        {
            DialogueText.text += ch;
            yield return new WaitForSeconds(typingTime);
        }
    }

    private void NextLine()
    {
        if (lineIndex < dialogueLines.Length - 1)
        {
            lineIndex++;
            StartCoroutine(ShowLine());
        }
        else
        {
            didDialogueStart = false;
            DialoguePanel.SetActive(false);
            Debug.Log("Diálogo terminado, panel desactivado");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Exclamation_Gray.SetActive(true);
            Debug.Log("Jugador entró en el área de diálogo");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Exclamation_Gray.SetActive(false);
            Debug.Log("Jugador salió del área de diálogo");
        }
    }
}