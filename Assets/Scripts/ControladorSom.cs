using UnityEngine;

public class ControladorSom : MonoBehaviour
{
    public AudioSource fonteSom;

    public AudioClip somTiro;
    public AudioClip somMotor;
    public AudioClip somLigarCarro;
    public AudioClip somPersonalizado;

    void Start()
    {
        if (fonteSom == null)
            fonteSom = GetComponent<AudioSource>();
    }

    public void TocarSom(AudioClip som)
    {
        if (som != null && fonteSom != null)
            fonteSom.PlayOneShot(som);
    }

    public void TocarSomTiro() => TocarSom(somTiro);
    public void TocarSomLigarCarro() => TocarSom(somLigarCarro);
    public void TocarSomPersonalizado() => TocarSom(somPersonalizado);

    // NOVOS métodos para o som do motor em loop
    public void IniciarSomMotor()
    {
        if (fonteSom != null && somMotor != null && !fonteSom.isPlaying)
        {
            fonteSom.clip = somMotor;
            fonteSom.loop = true;
            fonteSom.Play();
        }
    }

    public void PararSomMotor()
    {
        if (fonteSom != null && fonteSom.clip == somMotor && fonteSom.isPlaying)
        {
            fonteSom.Stop();
            fonteSom.clip = null;
            fonteSom.loop = false;
        }
    }
}
