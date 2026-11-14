using UnityEngine;

public class scriptPlacaSolar : MonoBehaviour
{
    public float energiaQueDa = 20f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        scriptEcobotController robot = collision.GetComponent<scriptEcobotController>();
        if (robot != null)
        {
            if (scriptControladorNivel.instancia != null)
            {
                scriptControladorNivel.instancia.SumarEnergia(energiaQueDa);
                // Si tienes un contador de placas en scriptControladorNivel, llámalo aquí:
                // scriptControladorNivel.instancia.RecogerPlaca();
            }

            Destroy(gameObject);
        }
    }
}