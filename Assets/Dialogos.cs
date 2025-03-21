using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogo : MonoBehaviour
{
    [SerializeField] private GameObject Exclamation_Gray;
    [SerializeField] private GameObject DialoguePanel;
    [SerializeField] private TMP_Text DialogueText;
    [SerializeField, TextArea(4, 6)] private string[] dialogueLines;

    private float typingTime = 0.05f;
    private bool isPlayerInRange;
    private bool didDialogueStart;
    private int lineIndex;
    private EnemyMovement enemyMovement;
    private EnemyValues enemyValues;
    private Rigidbody2D enemyRb;

    void Start()
    {
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
        if (isPlayerInRange && (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.JoystickButton7)))
        {
            if (!didDialogueStart)
            {
                StartDialogue();
            }
            else if (DialogueText.text == dialogueLines[lineIndex])
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
            Exclamation_Gray.SetActive(true);
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
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Exclamation_Gray.SetActive(true);
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
