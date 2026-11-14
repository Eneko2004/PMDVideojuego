using UnityEngine;

public class scriptColeccionable : MonoBehaviour
{
    // Variables configurables desde el Inspector
    [Header("Configuracion del Coleccionable")]
    public float energiaAporte = 30f;
    public bool requiereInteraccion = true; // Si es true, pide pulsar la P

    // Flag de control para saber si el player está dentro del trigger
    private bool playerEnRango = false;

    void Update()
    {
        // Control de input: interacción con la tecla 'P'
        if (requiereInteraccion && playerEnRango)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Recoger();
            }
        }
    }

    // Detección de colisión 2D mediante Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (EsElJugador(collision))
        {
            playerEnRango = true;

            // Si es un coleccionable pasivo (como una semilla normal), se coge al pasar
            if (!requiereInteraccion)
            {
                Recoger();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (EsElJugador(collision))
        {
            playerEnRango = false;
        }
    }

    // Lógica para aplicar el efecto y destruir el objeto de memoria
    private void Recoger()
    {
        // Llamada al GameManager mediante el patrón Singleton
        if (scriptControladorNivel.instancia != null)
        {
            scriptControladorNivel.instancia.SumarEnergia(energiaAporte);
        }

        Debug.Log("[PMDM Log]: Panel solar recogido con la tecla P (+Energía).");

        // Liberamos la instancia de la escena
        Destroy(gameObject);
    }

    // Método auxiliar para validar el tag o componente del personaje
    private bool EsElJugador(Collider2D col)
    {
        return col.GetComponent<scriptEcobotController>() != null || col.CompareTag("Player");
    }
}