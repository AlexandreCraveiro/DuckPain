using UnityEngine;

public class GeladoColetavel : MonoBehaviour
{
    private DispararGelados dispararGelados;
    private Rigidbody rb;

    [Tooltip("Velocidade abaixo da qual o gelado pode ser recolhido")]
    public float velocidadeRecolha = 0.2f;

    [Tooltip("Tempo mínimo após ser lançado antes de poder ser recolhido")]
    public float tempoMinimo = 0.5f;

    private float tempoDeNascimento;

    public void SetDispararGelados(DispararGelados controlador)
    {
        dispararGelados = controlador;
        tempoDeNascimento = Time.time;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PodeSerRecolhido())
        {
            dispararGelados.RecolherGelado();
            Destroy(gameObject);
        }
    }

    private bool PodeSerRecolhido()
    {
        return Time.time - tempoDeNascimento >= tempoMinimo && rb.linearVelocity.magnitude < velocidadeRecolha;
    }
}

