using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager instance;
    public AudioSource audioSource;

    

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayClick()
    {
        if (audioSource != null && audioSource.clip != null)
            audioSource.Play();
    }
}
