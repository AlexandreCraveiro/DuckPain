using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

    private IEnumerator SairComDelay()
    {
        yield return new WaitForSeconds(1f); // Espera 1 segundo
        SceneManager.LoadScene("Menu");
    }

    public void Sair()
    {
        StartCoroutine(SairComDelay());
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
