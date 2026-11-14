using UnityEngine;

public class scriptNubeCO2 : MonoBehaviour
{
    public float danio = 20f; // Quita un 20% de la barra

    [Header("Movimiento hacia EcoBot")]
    public float velocidad = 2f;          // Velocidad de persecución
    public float rangoDeteccion = 8f;     // Distancia a la que detecta a EcoBot
    public bool soloHorizontal = true;    // Si es true, solo se mueve a izquierda/derecha

    private Transform objetivo;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // Busca automáticamente al objeto que tenga tu scriptEcobotController
        scriptEcobotController robot = FindFirstObjectByType<scriptEcobotController>();
        if (robot != null)
        {
            objetivo = robot.transform;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (objetivo == null) return;

        float distancia = Vector2.Distance(transform.position, objetivo.position);

        // Si EcoBot entra dentro del rango, la nube va hacia él
        if (distancia <= rangoDeteccion)
        {
            Vector2 destino = objetivo.position;

            if (soloHorizontal)
            {
                destino.y = transform.position.y;
            }

            transform.position = Vector2.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);

            // Voltea el dibujo de la nube hacia donde está el personaje
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = objetivo.position.x < transform.position.x;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si choca contra el EcoBot
        scriptEcobotController robot = collision.GetComponent<scriptEcobotController>();
        if (robot != null)
        {
            // 1. Activa la animación de dolor
            robot.RecibirDanio();

            // 2. Le resta energía a la barra verde
            if (scriptControladorNivel.instancia != null)
            {
                scriptControladorNivel.instancia.RestarEnergia(danio);
            }
        }
    }
}