using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void Jogar()
    {
        UIAudioManager.instance.PlayClick();
        SceneManager.LoadScene("Map");
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
}
