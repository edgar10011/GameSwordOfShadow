using UnityEngine;

public class EnemyDeathDetector : MonoBehaviour
{
    private EnemyValues enemyValues;

    void Start()
    {
        // Obtener la referencia al componente EnemyValues
        enemyValues = GetComponent<EnemyValues>();

        // Verificar si el enemigo ya está muerto al inicio
        if (enemyValues.IsDead())
        {
            OnEnemyDeath();
        }
    }

    void Update()
    {
        // Si el enemigo muere durante la ejecución
        if (enemyValues.IsDead())
        {
            OnEnemyDeath();
        }
    }

    void OnEnemyDeath()
    {
        Debug.Log("Enemigo ha muerto. Activando cinemática...");

        // Envía un mensaje a todos los listeners para activar la cinemática
        SendMessage("OnEnemyKilled", this.gameObject, SendMessageOptions.DontRequireReceiver);

        // Desactivar este script para evitar que se envíe el mensaje repetidamente
        enabled = false;
    }
}