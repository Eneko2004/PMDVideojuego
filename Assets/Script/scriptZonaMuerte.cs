using UnityEngine;

public class scriptZonaMuerte : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<scriptEcobotController>() != null)
        {
            if (scriptControladorNivel.instancia != null)
            {
                scriptControladorNivel.instancia.ReiniciarNivel();
            }
        }
    }
}