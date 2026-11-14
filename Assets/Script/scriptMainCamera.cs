using UnityEngine;

public class scriptSeguirCamara : MonoBehaviour
{
    public Transform objetivo; // Aquí arrastraremos a EcoBot
    public float suavizado = 0.125f;
    public Vector3 desplazamiento = new Vector3(0, 1.5f, -10);

    void LateUpdate()
    {
        if (objetivo != null)
        {
            Vector3 posicionDeseada = objetivo.position + desplazamiento;
            transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado);
        }
    }
}