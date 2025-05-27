using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void Jogar()
    {
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
}
