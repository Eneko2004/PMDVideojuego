using UnityEngine;

public class scriptPlacaMeta : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si EcoBot pisa la placa de meta
        if (collision.GetComponent<scriptEcobotController>() != null)
        {
            if (scriptControladorNivel.instancia != null)
            {
                scriptControladorNivel.instancia.CompletarNivel();
            }
        }
    }
}