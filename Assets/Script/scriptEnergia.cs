using UnityEngine;

public class scriptEnergia : MonoBehaviour
{
    public float cantidadEnergia = 25f; // Sube un 25% la barra verde

    private void OnTriggerEnter2D(Collider2D collision)
    {
        scriptEcobotController robot = collision.GetComponent<scriptEcobotController>();
        if (robot != null)
        {
            if (scriptControladorNivel.instancia != null)
            {
                scriptControladorNivel.instancia.SumarEnergia(cantidadEnergia);
            }

            Destroy(gameObject);
        }
    }
}