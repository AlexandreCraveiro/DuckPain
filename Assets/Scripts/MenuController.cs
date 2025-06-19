using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MenuController : MonoBehaviour
{
    public GameObject videoScreen; 
    VideoPlayer videoPlayer;
    private void Start()
    {
        videoPlayer=videoScreen.GetComponent<VideoPlayer>();
        videoPlayer.Prepare();
    }
    public void Jogar()
    {
        UIAudioManager.instance.PlayClick();
        SceneManager.LoadScene("Map");
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("level", 0); // Reseta o nível para 0
        PlayerPrefs.Save(); // Salva as alterações
        videoScreen.SetActive(true); // Ativa a tela de vídeo
        videoPlayer.Play(); // Inicia o vídeo de introdução
        Invoke(nameof(Jogar), 35); // Aguarda o vídeo terminar para iniciar o jogo
        
    }

    public void Configuracoes()
    {
        SceneManager.LoadScene("Configuracoes");
    }

    public void Sair()
    {
        Application.Quit();
        Debug.Log("Jogo encerrado");
    }

    public void VoltarMenuPrincipal()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Creditos()
    {
        SceneManager.LoadScene("Credits");
    }
    public void DoInicioParaMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
