using UnityEngine;

public class GeladoColetavel : MonoBehaviour
{
    private DispararGelados dispararGelados;

    

    public void SetDispararGelados(DispararGelados disparar)
    {
        dispararGelados = disparar;
    }

    private void OnTriggerEnter(Collider other)
{
    Debug.Log("Trigger do gelado ativado por: " + other.name + ", tag: " + other.tag);

    if (other.CompareTag("Player"))
    {
        Debug.Log("Gelado detectou o jogador!");

        // Procura o DispararGelados no objeto ou nos pais
        DispararGelados disparar = other.GetComponentInParent<DispararGelados>();
        if (disparar != null)
        {
            disparar.RecolherGelado();
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("O jogador não tem o script DispararGelados!");
        }
    }
}


}
