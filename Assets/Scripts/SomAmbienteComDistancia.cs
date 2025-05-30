using UnityEngine;

public class SomAmbienteComDistancia : MonoBehaviour
{
    public Transform jogador;              // Referência ao jogador
    public Transform carrinha;             // Referência à carrinha (opcional)
    public bool jogadorDentroDaCarrinha;   // Isto será alterado externamente
    public float distanciaMaxima = 20f;

    private AudioSource audioFonte;

    void Start()
    {
        audioFonte = GetComponent<AudioSource>();
        audioFonte.loop = true;
        audioFonte.playOnAwake = false;
        audioFonte.spatialBlend = 0f; // 2D (volume manual)
    }

    void Update()
    {
        Transform referencia = jogadorDentroDaCarrinha ? carrinha : jogador;

        if (referencia == null) return;

        float distancia = Vector3.Distance(referencia.position, transform.position);

        if (distancia < distanciaMaxima)
        {
            float volume = 1f - (distancia / distanciaMaxima);
            audioFonte.volume = volume;

            if (!audioFonte.isPlaying)
                audioFonte.Play();
        }
        else
        {
            if (audioFonte.isPlaying)
                audioFonte.Stop();
        }
        if (Time.timeScale == 0)
        {
            audioFonte.Pause(); // Pausa o som se o jogo estiver pausado
        }
        else
        {
            audioFonte.UnPause(); // Retoma o som se o jogo estiver ativo
        }
    }
}
