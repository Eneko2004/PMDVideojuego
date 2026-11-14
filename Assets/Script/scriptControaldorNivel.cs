using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class scriptControladorNivel : MonoBehaviour
{
    public static scriptControladorNivel instancia;

    [Header("Energía")]
    public float energiaMaxima = 100f;
    public float energiaActual;
    public Slider barraEnergia;
    public Image imagenRellenoVerde; // El objeto 'Fill' para parpadear en rojo

    [Header("Placas Solares")]
    public int placasTotales;
    public int placasRecogidas = 0;

    [Header("Efectos")]
    private Color colorOriginal;
    private Coroutine corrutinaAnimacion;

    [Header("Panel de Victoria")]
    public GameObject panelVictoria;

    void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        Time.timeScale = 1f;
        energiaActual = energiaMaxima;

        // Cuenta automáticamente todas las placas solares de la escena
        placasTotales = FindObjectsByType<scriptPlacaSolar>(FindObjectsSortMode.None).Length;

        if (barraEnergia != null)
        {
            barraEnergia.maxValue = energiaMaxima;
            barraEnergia.value = energiaActual;
        }

        if (imagenRellenoVerde != null)
        {
            colorOriginal = imagenRellenoVerde.color;
        }

        if (panelVictoria != null)
        {
            panelVictoria.SetActive(false);
        }
    }

    // Restar energía con animación y parpadeo rojo
    public void RestarEnergia(float cantidad)
    {
        energiaActual -= cantidad;
        if (energiaActual < 0) energiaActual = 0;

        if (corrutinaAnimacion != null) StopCoroutine(corrutinaAnimacion);
        corrutinaAnimacion = StartCoroutine(AnimarEfectoDanio(energiaActual));

        if (energiaActual <= 0)
        {
            ReiniciarNivel();
        }
    }

    // Sumar energía con animación suave
    public void SumarEnergia(float cantidad)
    {
        energiaActual += cantidad;
        if (energiaActual > energiaMaxima) energiaActual = energiaMaxima;

        if (corrutinaAnimacion != null) StopCoroutine(corrutinaAnimacion);

        // Restaura el color verde por si se interrumpió el parpadeo rojo del daño
        if (imagenRellenoVerde != null)
            imagenRellenoVerde.color = colorOriginal;

        corrutinaAnimacion = StartCoroutine(AnimarBarraSuave(energiaActual));
    }

    // Recoger placa solar y sumar su energía
    public void RecogerPlaca(float energiaExtra)
    {
        placasRecogidas++;
        SumarEnergia(energiaExtra);
    }

    public bool TieneTodasLasPlacas()
    {
        return placasRecogidas >= placasTotales;
    }

    // Corrutina que hace el efecto de daño
    IEnumerator AnimarEfectoDanio(float valorObjetivo)
    {
        if (imagenRellenoVerde != null)
            imagenRellenoVerde.color = Color.red;

        float valorInicial = barraEnergia.value;
        float tiempo = 0f;
        float duracion = 0.3f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            barraEnergia.value = Mathf.Lerp(valorInicial, valorObjetivo, tiempo / duracion);
            yield return null;
        }

        barraEnergia.value = valorObjetivo;

        if (imagenRellenoVerde != null)
            imagenRellenoVerde.color = colorOriginal;
    }

    IEnumerator AnimarBarraSuave(float valorObjetivo)
    {
        float valorInicial = barraEnergia.value;
        float tiempo = 0f;
        float duracion = 0.25f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            barraEnergia.value = Mathf.Lerp(valorInicial, valorObjetivo, tiempo / duracion);
            yield return null;
        }

        barraEnergia.value = valorObjetivo;
    }

    public void CompletarNivel()
    {
        if (panelVictoria != null) panelVictoria.SetActive(true);
        Time.timeScale = 0f;
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}