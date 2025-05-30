using UnityEngine;

public class DispararGelados : MonoBehaviour
{
    public GameObject geladoPrefab;

    public Transform geladoSpawnPlayer; // local onde o gelado nasce se estiver fora da carrinha
    public Transform geladoSpawnCarro;
    public Transform pontoDisparoPlayer;
    public Transform pontoDisparoCarrinha;

    public ControladorSom somControlador;

    public float forcaDisparo = 700f;
    public float tempoEntreTiros = 1f;

    private float proximoTiro = 0f;

    public bool jogadorDentroDoCarro = false;

    HintManager hintManager;

    private void Start()
    {
        hintManager = FindAnyObjectByType<HintManager>();
    }

    // Agora privado
    private bool temGelado = true;

    public bool TemGelado()
    {
        return temGelado;
    }

    public void SetTemGelado(bool valor)
    {
        temGelado = valor;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // tecla para disparar
        {
            Disparar();
        }
    }

    void Disparar()
    {
        if (Time.timeScale == 0) return; // não dispara se o jogo estiver pausado
        // Fora da carrinha, só pode disparar se tiver gelado
        if (!jogadorDentroDoCarro && !temGelado)
        {
            Debug.Log("Não tens gelado para disparar. Volta à carrinha para recarregar.");
            hintManager.ShowHint("Não tens gelado para disparar. Volta à carrinha para recarregar.");
            return;
        }

        // Controlar tempo entre tiros
        if (Time.time < proximoTiro) return;
        proximoTiro = Time.time + tempoEntreTiros;

        // Define o ponto de disparo
        Transform spawnPoint = jogadorDentroDoCarro ? pontoDisparoCarrinha : pontoDisparoPlayer;

        // Instancia o gelado
        GameObject gelado = Instantiate(geladoPrefab, spawnPoint.position, spawnPoint.rotation);
        Destroy(gelado, 5f); // destrói o gelado após 5 segundos para evitar acumulação
        if (jogadorDentroDoCarro == false)
            gelado.tag = "FreeIceCream";
        // Liga o script DispararGelados ao gelado
        GeladoColetavel geladoScript = gelado.GetComponent<GeladoColetavel>();
        if (geladoScript != null)
        {
            geladoScript.SetDispararGelados(this);
        }
        else
        {
            Debug.LogWarning("Prefab gelado não tem script GeladoColetavel!");
        }

        // Aplica força
        Rigidbody rb = gelado.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(spawnPoint.forward * forcaDisparo);
        }

        // Toca som
        if (somControlador != null)
            somControlador.TocarSomTiro();

        // Fora da carrinha, consome o gelado
        if (!jogadorDentroDoCarro)
        {
            temGelado = false;
        }
    }

    // Só recarrega se estiver dentro da carrinha
    public void RecolherGelado()
    {
        temGelado = true;
        Debug.Log("Gelado recolhido. Agora podes disparar de novo.");
    }


    public void EntrouNoCarro()
    {
        jogadorDentroDoCarro = true;
        temGelado = true; // Recarrega automaticamente ao entrar
        Debug.Log("Entraste na carrinha. Gelado recarregado.");
    }

    public void SaiuDoCarro()
    {
        jogadorDentroDoCarro = false;
        Debug.Log("Saíste da carrinha.");
    }
}

