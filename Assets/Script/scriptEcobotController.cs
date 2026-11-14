using UnityEngine;

public class scriptEcobotController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 6f;
    public float fuerzaSalto = 11f;

    [Header("Detección de Suelo")]
    public Transform detectorSuelo;
    public LayerMask capaSuelo;
    public float radioDeteccion = 0.25f;

    [Header("Disparo")]
    public GameObject prefabBala;
    public Transform puntoDisparo;

    [Header("Evolución por Semillas")]
    public int semillasRecogidas = 0;
    public int semillasParaEvolucionar = 3;
    public float multiplicadorTamanoEvolucion = 1.25f; // Crece un 25%
    public Sprite spriteEvolucionado;                  // Opcional: sprite evolucionado
    private bool evolucionado = false;
    private float escalaActual = 1f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private bool enSuelo;
    private float direccionMirada = 1f; // 1 = derecha, -1 = izquierda
    private float posXOriginalPuntoDisparo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (puntoDisparo != null)
        {
            posXOriginalPuntoDisparo = Mathf.Abs(puntoDisparo.localPosition.x);
        }
    }

    void Update()
    {
        // ── 1. Entrada táctil (Android) ──────────────────────────────
        bool tactilIzquierda = false;
        bool tactilDerecha   = false;
        bool tactilSaltar    = false;
        bool tactilDisparar  = false;

        if (EntradaMovil.Instancia != null)
        {
            tactilIzquierda = EntradaMovil.Instancia.MoverIzquierda;
            tactilDerecha   = EntradaMovil.Instancia.MoverDerecha;
            tactilSaltar    = EntradaMovil.Instancia.SaltarPulsado;
            tactilDisparar  = EntradaMovil.Instancia.DispararPulsado;
        }

        // ── 2. Movimiento horizontal (Teclado + Táctil) ──────────────
        float movimientoH = Input.GetAxisRaw("Horizontal");

        if (tactilIzquierda) movimientoH = -1f;
        if (tactilDerecha)   movimientoH =  1f;

        rb.linearVelocity = new Vector2(movimientoH * velocidad, rb.linearVelocity.y);

        // Girar el sprite usando flipX (mantiene localScale siempre en positivo)
        if (movimientoH > 0)
        {
            if (spriteRenderer != null) spriteRenderer.flipX = false;
            direccionMirada = 1f;
            ActualizarLadoPuntoDisparo();
        }
        else if (movimientoH < 0)
        {
            if (spriteRenderer != null) spriteRenderer.flipX = true;
            direccionMirada = -1f;
            ActualizarLadoPuntoDisparo();
        }

        // ── 3. Comprobar si toca el suelo ────────────────────────────
        if (detectorSuelo != null)
        {
            enSuelo = Physics2D.OverlapCircle(detectorSuelo.position, radioDeteccion, capaSuelo);
        }

        // ── 4. Salto (Teclado + Táctil) ──────────────────────────────
        bool saltarTeclado = Input.GetButtonDown("Jump");

        if ((saltarTeclado || tactilSaltar) && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        }

        // ── 5. Disparo (Teclado E + Táctil) ──────────────────────────
        if (Input.GetKeyDown(KeyCode.E) || tactilDisparar)
        {
            Disparar();
        }

        // ── 6. Animator ───────────────────────────────────────────────
        if (anim != null)
        {
            anim.SetFloat("Velocidad", Mathf.Abs(movimientoH));
            anim.SetBool("Saltando", !enSuelo);
        }
    }

    void ActualizarLadoPuntoDisparo()
    {
        if (puntoDisparo != null)
        {
            puntoDisparo.localPosition = new Vector3(
                posXOriginalPuntoDisparo * direccionMirada,
                puntoDisparo.localPosition.y,
                puntoDisparo.localPosition.z
            );
        }
    }

    void Disparar()
    {
        if (anim != null)
        {
            anim.SetTrigger("Disparar");
        }

        if (prefabBala != null && puntoDisparo != null)
        {
            GameObject bala = Instantiate(prefabBala, puntoDisparo.position, Quaternion.identity);

            // Si está evolucionado, la bala de aire limpio también es más grande
            if (evolucionado)
            {
                bala.transform.localScale *= 1.4f;
            }

            scriptProyectilAire scriptBala = bala.GetComponent<scriptProyectilAire>();
            if (scriptBala != null)
            {
                scriptBala.IniciarDireccion(new Vector2(direccionMirada, 0));
            }
        }
    }

    // Método para cuando reciba daño de una nube
    public void RecibirDanio()
    {
        if (anim != null)
        {
            anim.SetTrigger("RecibirDanio");
        }
    }

    // Método llamado por scriptSemilla al recoger una semilla
    public void RecogerSemilla()
    {
        semillasRecogidas++;

        if (semillasRecogidas >= semillasParaEvolucionar && !evolucionado)
        {
            Evolucionar();
        }
    }

    void Evolucionar()
    {
        evolucionado = true;
        escalaActual = multiplicadorTamanoEvolucion;

        // Aplica la nueva escala siempre en positivo
        transform.localScale = new Vector3(escalaActual, escalaActual, 1f);

        // Mejora sus habilidades al evolucionar
        velocidad += 1.5f;
        fuerzaSalto += 1.5f;

        // Cambia el sprite si asignaste uno en el Inspector
        if (spriteEvolucionado != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = spriteEvolucionado;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (detectorSuelo != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(detectorSuelo.position, radioDeteccion);
        }
    }
}