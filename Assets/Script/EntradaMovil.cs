using UnityEngine;

public class EntradaMovil : MonoBehaviour
{
    public static EntradaMovil Instancia { get; private set; }

    public bool MoverIzquierda { get; private set; }
    public bool MoverDerecha   { get; private set; }
    public bool Saltar         { get; private set; }
    public bool Disparar       { get; private set; }

    private bool _saltarPulsado;
    private bool _dispararPulsado;
    public bool SaltarPulsado   { get { bool v = _saltarPulsado;   _saltarPulsado  = false; return v; } }
    public bool DispararPulsado { get { bool v = _dispararPulsado; _dispararPulsado = false; return v; } }

    void Awake()
    {
        if (Instancia != null && Instancia != this) { Destroy(gameObject); return; }
        Instancia = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AlPulsarIzquierda()  { MoverIzquierda = true;  }
    public void AlSoltarIzquierda()  { MoverIzquierda = false; }

    public void AlPulsarDerecha()    { MoverDerecha = true;  }
    public void AlSoltarDerecha()    { MoverDerecha = false; }

    public void AlPulsarSaltar()     { Saltar = true;  _saltarPulsado   = true; }
    public void AlSoltarSaltar()     { Saltar = false; }

    public void AlPulsarDisparar()   { Disparar = true;  _dispararPulsado = true; }
    public void AlSoltarDisparar()   { Disparar = false; }

    void OnApplicationFocus(bool tieneFoco)
    {
        if (!tieneFoco) MoverIzquierda = MoverDerecha = Saltar = Disparar = false;
    }
}