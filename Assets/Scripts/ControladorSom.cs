using Unity.VisualScripting;
using UnityEngine;

public class ControladorSom : MonoBehaviour
{
    public AudioSource fonteSom;
    public AudioSource fonteSomMusica;

    public AudioClip somTiro;
    public AudioClip somMotor;
    public AudioClip somLigarCarro;
    public AudioClip somPersonalizado;
    public AudioClip somTravagem;

    public AudioSource somAmbienteCidade;

    void Start()
    {
        if (fonteSom == null)
            fonteSom = GetComponent<AudioSource>();
        //cria um audiosource só para a música do carro
        if (fonteSomMusica == null)
            fonteSomMusica=this.AddComponent<AudioSource>();

    }

    public void TocarSom(AudioClip som, float volume = 1f)
    {
        if (som != null && fonteSom != null)
            fonteSom.PlayOneShot(som, volume);
    }

    public void TocarSomTravagem(float volume)
    {
        TocarSom(somTravagem, volume);
    }

    public void TocarSomTiro() => TocarSom(somTiro);
    public void TocarSomLigarCarro() => TocarSom(somLigarCarro);

    // ✅ Toca o som personalizado (por exemplo, música), substituindo qualquer outro som em reprodução
    public void IniciarSomPersonalizado(bool loop = true)
    {
        if (fonteSomMusica != null && somPersonalizado != null)
        {
            fonteSomMusica.Stop(); // Interrompe qualquer som anterior (ex: motor)
            fonteSomMusica.clip = somPersonalizado;
            fonteSomMusica.loop = loop;
            fonteSomMusica.Play();
            Debug.Log("🎵 Música personalizada a tocar.");
        }
    }

    // ✅ Para a música personalizada se estiver a tocar
    public void PararSomPersonalizado()
    {
        if (fonteSomMusica != null && fonteSomMusica.clip == somPersonalizado && fonteSomMusica.isPlaying)
        {
            fonteSomMusica.Stop();
            fonteSomMusica.clip = null;
            fonteSomMusica.loop = false;
            Debug.Log("🛑 Música personalizada parada.");
        }
    }
    public bool ATocarMusica()
    {
        return fonteSomMusica != null && fonteSomMusica.isPlaying && fonteSomMusica.clip == somPersonalizado;
    }
    // ✅ Toca o som do motor em loop
    public void IniciarSomMotor()
    {
        if (fonteSom != null && somMotor != null && (!fonteSom.isPlaying || fonteSom.clip != somMotor))
        {
            fonteSom.Stop(); // Garante transição limpa
            fonteSom.clip = somMotor;
            fonteSom.loop = true;
            fonteSom.Play();
        }
    }

    // ✅ Para o som do motor
    public void PararSomMotor()
    {
        if (fonteSom != null && fonteSom.clip == somMotor && fonteSom.isPlaying)
        {
            fonteSom.Stop();
            fonteSom.clip = null;
            fonteSom.loop = false;
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0)
        {
            somAmbienteCidade.Pause(); // Pausa o som se o jogo estiver pausado
        }
        else
        {
            somAmbienteCidade.UnPause(); // Retoma o som se o jogo estiver ativo
        }
    }
}
