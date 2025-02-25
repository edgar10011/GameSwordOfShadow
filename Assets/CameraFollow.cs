using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // Personaje a seguir
    [SerializeField] private float smoothSpeed = 5f; // Velocidad de suavizado
    [SerializeField] private Vector3 offset; // Ajuste de posición de la cámara

    // Límites de la cámara
    [SerializeField] private float derechaMax = 19.9f;
    [SerializeField] private float izquierdaMax = -17.93f;
    [SerializeField] private float alturaMax = 16.58f;
    [SerializeField] private float alturaMin = -26.52f;

    void LateUpdate()
    {
        if (target == null) return;

        // Obtener la posición deseada de la cámara
        Vector3 desiredPosition = target.position + offset;

        // Aplicar restricciones de límites
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, izquierdaMax, derechaMax);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, alturaMin, alturaMax);

        // Mantener la posición Z de la cámara para evitar problemas de renderizado
        desiredPosition.z = transform.position.z;

        // Interpolación para un movimiento suave
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
