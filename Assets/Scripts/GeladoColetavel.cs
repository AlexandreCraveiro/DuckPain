using UnityEngine;

public class GeladoColetavel : MonoBehaviour
{
    private DispararGelados dispararGelados;

    private bool podeSerRecolhido = true;

    public void SetDispararGelados(DispararGelados script)
    {
        dispararGelados = script;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!podeSerRecolhido) return;

        if (other.CompareTag("Player")) // Certifica-te que o jogador tem esta Tag
        {
            if (dispararGelados != null)
            {
                dispararGelados.RecolherGelado();
                Destroy(gameObject); // Só destrói quando for recolhido
            }
        }
    }
}