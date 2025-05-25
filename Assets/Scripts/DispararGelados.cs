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
        if (!jogadorDentroDoCarro && !temGelado)
        {
            Debug.Log("Não tens gelado para disparar.");
            return;
        }

        if (Time.time < proximoTiro) return;
        proximoTiro = Time.time + tempoEntreTiros;

        Transform spawnPoint = jogadorDentroDoCarro ? pontoDisparoCarrinha : pontoDisparoPlayer;

        GameObject gelado = Instantiate(geladoPrefab, spawnPoint.position, spawnPoint.rotation);

        GeladoColetavel geladoScript = gelado.GetComponent<GeladoColetavel>();
        if (geladoScript != null)
        {
            geladoScript.SetDispararGelados(this);
        }
        else
        {
            Debug.LogWarning("Prefab gelado não tem script GeladoColetavel!");
        }

        Rigidbody rb = gelado.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(spawnPoint.forward * forcaDisparo);
        }

        if (somControlador != null)
            somControlador.TocarSomTiro();

        if (!jogadorDentroDoCarro)
        {
            temGelado = false;
        }
    }

    // Método para ser chamado quando o gelado é recolhido
    public void RecolherGelado()
    {
        Debug.Log("Gelado recolhido. Agora podes disparar de novo.");
        temGelado = true;
    }

    public void EntrouNoCarro()
    {
        jogadorDentroDoCarro = true;
    }

    public void SaiuDoCarro()
    {
        jogadorDentroDoCarro = false;
    }
}
