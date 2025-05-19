using UnityEngine;

public class DispararGelados : MonoBehaviour
{
    public GameObject geladoPrefab;

    public Transform geladoSpawnPlayer; // local onde o gelado nasce se estiver fora da carrinha
    public Transform geladoSpawnCarro;
    public Transform pontoDisparoPlayer;
    public Transform pontoDisparoCarrinha;

    public float forcaDisparo = 700f;
    public float tempoEntreTiros = 1f;

    private float proximoTiro = 0f;

    public bool jogadorDentroDoCarro = false;





    void Start()
    {
        
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // tecla para disparar, por exemplo
        {
            Disparar();
        }
    }

    void Disparar()
{
    if (Time.time < proximoTiro) return;
    proximoTiro = Time.time + tempoEntreTiros;

    Transform spawnPoint = jogadorDentroDoCarro ? pontoDisparoCarrinha : pontoDisparoPlayer;

    GameObject gelado = Instantiate(geladoPrefab, spawnPoint.position, spawnPoint.rotation);
    Rigidbody rb = gelado.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.AddForce(spawnPoint.forward * forcaDisparo);
    }
}





    // Estes métodos são chamados pelo sistema externo
    public void EntrouNoCarro()
    {
        jogadorDentroDoCarro = true;
    }

    public void SaiuDoCarro()
    {
        jogadorDentroDoCarro = false;
    }
}