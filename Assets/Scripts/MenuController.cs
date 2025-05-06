using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void Jogar()
    {
        SceneManager.LoadScene("NomeDaCenaDoJogo");
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
}
