using UnityEngine;

using UnityEngine;

public class ZonaRestrita : MonoBehaviour
{
    public int nivelMinimo = 1; // nível mínimo necessário para entrar
    public GameObject[] zonas;
    public GameObject[] barreira;
    private int nivelAtual;

    void Start()
    {
        nivelAtual = PlayerPrefs.GetInt("level");
        if (nivelAtual >= nivelMinimo)
            {
                foreach (GameObject b in zonas)
                {
                    b.SetActive(false);
                }
            }
            else
            {
                foreach (GameObject b in zonas)
                {
                    b.SetActive(true);
                }
            }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(nivelAtual);
            if (nivelAtual >= nivelMinimo)
            {
                Debug.Log("Entrada permitida.");
                foreach (GameObject b in barreira)
                {
                    b.SetActive(false);
                }
            }
            else
            {
                Debug.Log("Nível insuficiente. Entrada negada.");
                // Exemplo: empurrar o jogador de volta
                foreach (GameObject b in barreira)
                {
                    b.SetActive(true);
                }
            }
        }
    }
}

