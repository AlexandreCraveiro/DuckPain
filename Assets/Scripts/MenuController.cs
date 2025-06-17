using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void Jogar()
    {
        UIAudioManager.instance.PlayClick();
        SceneManager.LoadScene("Map");
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("level", 0); // Reseta o n�vel para 0
        PlayerPrefs.Save(); // Salva as altera��es
        Jogar(); // Inicia o jogo
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
