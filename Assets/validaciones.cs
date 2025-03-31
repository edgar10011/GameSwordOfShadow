using System.Collections;
using UnityEngine;
using TMPro;
public class Validaciones : MonoBehaviour
{
    [SerializeField] private GameObject Exclamation_Gray;
    [SerializeField] private GameObject DialoguePanel;
    [SerializeField] private TMP_Text DialogueText;
    [SerializeField, TextArea(4, 6)] private string[] dialogueLines;

    private float typingTime = 0.05f;
    private bool isPlayerInRange;
    private bool didDialogueStart;
    private static bool hasDialogueBeenShown = false; // Cambiado a static para persistencia
    private int lineIndex;
    private EnemyMovement enemyMovement;
    private EnemyValues enemyValues;
    private Rigidbody2D enemyRb;

    void Start()
    {
        if (hasDialogueBeenShown)
        {
            Destroy(gameObject); // Destruir si ya se mostró antes
            return;
        }

        enemyMovement = GetComponentInParent<EnemyMovement>();
        enemyValues = GetComponentInParent<EnemyValues>();
        enemyRb = GetComponentInParent<Rigidbody2D>();
        if (enemyRb != null)
        {
            enemyRb.bodyType = RigidbodyType2D.Kinematic;
        }
        if (enemyMovement != null)
        {
            enemyMovement.enabled = false;
        }
        if (enemyValues != null)
        {
            enemyValues.enabled = false;
        }
    }
    void Update()
    {
        if (didDialogueStart && (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.JoystickButton7)))
        {
            if (DialogueText.text == dialogueLines[lineIndex])
            {
                NextDialogueLine();
            }
            else
            {
                StopAllCoroutines();
                DialogueText.text = dialogueLines[lineIndex];
            }
        }
    }

    private void StartDialogue()
    {
        didDialogueStart = true;
        DialoguePanel.SetActive(true);
        Exclamation_Gray.SetActive(false);
        lineIndex = 0;
        Time.timeScale = 0f;
        StartCoroutine(ShowLine());
    }

    private void NextDialogueLine()
    {
        lineIndex++;
        if (lineIndex < dialogueLines.Length)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            didDialogueStart = false;
            DialoguePanel.SetActive(false);
            Exclamation_Gray.SetActive(false); // Asegurar que desaparezca
            Time.timeScale = 1f;
            if (enemyRb != null)
            {
                enemyRb.bodyType = RigidbodyType2D.Dynamic;
            }
            if (enemyMovement != null)
            {
                enemyMovement.enabled = true;
                enemyMovement.StartPatrol();
            }
            if (enemyValues != null)
            {
                enemyValues.enabled = true;
            }
            hasDialogueBeenShown = true; // Se guarda para que no vuelva a mostrarse
            Destroy(gameObject); // Destruir el objeto para que no reaparezca
        }
    }

    private IEnumerator ShowLine()
    {
        DialogueText.text = string.Empty;
        foreach (char ch in dialogueLines[lineIndex])
        {
            DialogueText.text += ch;
            yield return new WaitForSecondsRealtime(typingTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasDialogueBeenShown)
        {
            isPlayerInRange = true;
            Exclamation_Gray.SetActive(true);
            if (!didDialogueStart) // Evita que se repita si ya empezó
            {
                StartDialogue();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Exclamation_Gray.SetActive(false);
        }
    }
}
