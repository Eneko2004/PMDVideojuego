using UnityEngine;

public class scriptProyectilAire : MonoBehaviour
{
    public float velocidad = 10f;
    public float tiempoVida = 2f; // Se destruye a los 2 segundos si no choca
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    // Le da dirección a la bala al nacer
    public void IniciarDireccion(Vector2 direccion)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direccion * velocidad;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si choca con una nube de contaminacion
        if (collision.CompareTag("NubeCO2"))
        {
            Destroy(collision.gameObject); // Destruye la nube
            Destroy(gameObject);           // Destruye la bala
        }
        // Si choca contra el suelo o paredes
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}