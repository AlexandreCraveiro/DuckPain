using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuJogo : MonoBehaviour
{
    public GameObject menuJogo;
    public string nomedomenuprincipal;
    public void Pausa()
    {
        menuJogo.SetActive(true);   //mostra o menu
        Time.timeScale = 0;            //para o tempo
        Cursor.lockState = CursorLockMode.None; //mostrar o cursor do rato
    }
    public void Continuar()
    {
        menuJogo.SetActive(false);  //esconde o menu
        Time.timeScale = 1;            //retoma o tempo
        Cursor.lockState = CursorLockMode.Locked; //esconde o cursor do rato
    }
    public void Sair()
    {
        SceneManager.LoadScene(nomedomenuprincipal);
    }

    // Update is called once per frame
    void Update()
    {
        if (SistemaInput.instance.TeclaEsc)
        {
            if (Time.timeScale == 0)
                Continuar();
            else
                Pausa();
        }
    }
}
