using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuJogo : MonoBehaviour
{
    public GameObject menuJogo;
    public string nomedomenuprincipal;
    ObjectiveZone objectiveZone;
    public GameObject PainelControlos;

    public void Start()
    {
        objectiveZone = FindAnyObjectByType<ObjectiveZone>();
    }
    public void Recomecar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //recarrega a cena atual
    }
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
        UIAudioManager.instance.PlayClick();
        SceneManager.LoadScene(nomedomenuprincipal);
    }

    // Update is called once per frame
    void Update()
    {
        if (objectiveZone.apanhado)
        {
            return; // Se o jogador foi apanhado, n�o processa mais nada
        }
        if (SistemaInput.instance.TeclaEsc)
        {
            if (Time.timeScale == 0)
                Continuar();
            else
                Pausa();
        }
    }

     public void MostrarControlos()
    {
        PainelControlos.SetActive(true);
    }

    public void FecharControlos()
    {
        PainelControlos.SetActive(false);
    }
}
