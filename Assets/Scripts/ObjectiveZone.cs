using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;

public class ObjectiveZone : MonoBehaviour
{
    public TextMeshProUGUI resultsText;
    public TextMeshProUGUI score;
    public TextMeshProUGUI successMessage;
    public int total = 3;
    public int wrongKidsCount = 0;
    public int scoreCount = 0;
    public GameObject[] barreira;
    public GameObject painelsucesso;
    public GameObject painellose;
    public GameObject[] kids;
    public int nivel;
    public bool apanhado;
    HintManager hintManager;
    public VideoClip[] videoClips;
    public GameObject cutscenePanel;
    VideoPlayer videoPlayer;
    public GameObject panelFinal;
    IEnumerator ShowTemporaryMessage(string msg, float duration, CaptureManager manager)
    {
        resultsText.text = msg;
        yield return new WaitForSeconds(duration);
        resultsText.text = "";
        manager.ResetCounters();
    }
    private void Awake()
    {
        nivel = PlayerPrefs.GetInt("level");
        Debug.Log("N�vel carregado: " + nivel);
        AbreNivel(nivel);
        Time.timeScale = 1f; // Garante que o jogo comeca com o tempo normal
        hintManager = FindAnyObjectByType<HintManager>();
        Debug.Log("Level: " + PlayerPrefs.GetInt("level"));
        setLevelTotal();

    }

    private void setLevelTotal() {
        switch(nivel) {
            case 0:
                total = 3;
                break;
            case 1:
                total = 5;
                break;
            case 2:
                total = 2;
                break;
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("EnterVehicle")) {
            CaptureManager manager = other.GetComponentInChildren<CaptureManager>();
            if (manager != null) {
                if (manager.getCorrectedCount() + manager.getIncorrectedCount() == 0) {
                    hintManager.ShowHint("Captura crian�as e volta aqui para as entregar.", 5f);
                    return;
                }
                string message = "Foram capturadas " + manager.getCorrectedCount().ToString() + " corretas e " + manager.getIncorrectedCount().ToString() + " incorretas.";
                scoreCount = scoreCount + manager.getCorrectedCount();
                wrongKidsCount = wrongKidsCount + manager.getIncorrectedCount();
                manager.ResetCounters();
                //StartCoroutine(ShowTemporaryMessage(message, 2f, manager));
                hintManager.ShowHint(message, 5f);
                //Verifica se acabou o nível
                if (scoreCount == total && wrongKidsCount == 0)
                {

                    // NivelConcluido();
                    FindAnyObjectByType<PlayerInteraction>().ExitCar(); //sai do carro
                    Time.timeScale = 0f; // Pausa o jogo
                    StartCoroutine(nameof(ShowCutScene));
                }
                else if (scoreCount == total && wrongKidsCount > 0) {
                    Cursor.lockState = CursorLockMode.None;
                    painellose.SetActive(true);
                }
            } else {
                Debug.Log("CaptureManager not found");
            }
        }
    }



    private void AbreNivel(int nivel)
    {
        if(nivel > 2) {
            return;
        }

        Debug.Log("Abrindo n�vel: " + nivel);


            foreach (GameObject kid in kids)
            {
                kid.SetActive(false);
            }
            kids[nivel].SetActive(true);

        if (nivel > 0)
        {
            Debug.Log("Destruindo barreira");
            Destroy(barreira[0]);
            if (nivel > 1) {
                Destroy(barreira[1]);
            }
            Cursor.lockState = CursorLockMode.None;
            scoreCount = 0;
            total = 5;
        }



    }

    IEnumerator ShowCutScene()
    {
        float duracao = (float)videoClips[nivel].length;
        Debug.Log("Showing cutscene for level: " + nivel + " com a duração de "+ duracao);
        cutscenePanel.SetActive(true);
        videoPlayer = cutscenePanel.GetComponentInChildren<VideoPlayer>();
        videoPlayer.clip = videoClips[nivel];
        videoPlayer.Play();
        yield return new WaitForSecondsRealtime (duracao); // Espera a duração do vídeo + 1 segundo
        Debug.Log("Cutscene ended, closing panel.");
        cutscenePanel.SetActive(false);
        videoPlayer.Stop();
        NivelConcluido();
    }

    public void CloseCutScene()
    {
        cutscenePanel.SetActive(false);
        videoPlayer.Stop();
        NivelConcluido();
    }
    private void NivelConcluido()
    {
        nivel++;
        //Verificar se é o ultimo nível
        if (nivel > 2)
        {
            panelFinal.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            return;
        }
        successMessage.text = "Nivel " + nivel + " concluido com sucesso!";
        PlayerPrefs.SetInt("level", nivel);
        PlayerPrefs.Save();
        Debug.Log("Nivel: " + nivel);
        Debug.Log("Nivel salvo: " + PlayerPrefs.GetInt("level"));
        painelsucesso.SetActive(true);
        AbreNivel(nivel);
        Cursor.lockState = CursorLockMode.None;
        setLevelTotal();
    }
    void Update() {
        score.text = scoreCount.ToString() + "/" + total.ToString();
        if(nivel > 2) {
            score.text = "Todos os níveis concluidos!";
        }
    }
}
