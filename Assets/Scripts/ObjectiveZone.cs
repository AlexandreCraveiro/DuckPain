using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEditor.Build;

public class ObjectiveZone : MonoBehaviour
{
    public TextMeshProUGUI resultsText;
    public TextMeshProUGUI score;
    public TextMeshProUGUI successMessage;
    public int total = 3;
    public int wrongKidsCount = 0;
    public int scoreCount = 0;
    public GameObject barreira;
    public GameObject painelsucesso;
    public GameObject painellose;
    public GameObject[] kids;
    public int nivel;
    public bool apanhado;
    HintManager hintManager;
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
        Time.timeScale = 1f; // Garante que o jogo come�a com o tempo normal
        hintManager = FindAnyObjectByType<HintManager>();
        Debug.Log("Level: " + PlayerPrefs.GetInt("level"));

        switch(nivel) {
            case 0:
                total = 3;
                break;
            case 1:
                total = 5;
                break;
            case 2:
                total = 1;
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
                if (scoreCount == total && wrongKidsCount == 0)
                {
                    successMessage.text = "Nivel " + (nivel + 1) + " concluido com sucesso!";
                    PlayerPrefs.SetInt("level", nivel + 1);
                    Debug.Log("Nivel: " + nivel);
                    Debug.Log("Nivel salvo: " + PlayerPrefs.GetInt("level"));
                    PlayerPrefs.Save();
                    // nivel++;
                    Time.timeScale = 0f; // Pausa o jogo
                    
                    painelsucesso.SetActive(true);
                    AbreNivel(nivel+1);
                    Cursor.lockState = CursorLockMode.None;
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
        Debug.Log("Abrindo n�vel: " + nivel);


            foreach (GameObject kid in kids)
            {
                kid.SetActive(false);
            }
            kids[nivel].SetActive(true);

        if (nivel > 0)
        {
            Debug.Log("Destruindo barreira");
            Destroy(barreira);
            Cursor.lockState = CursorLockMode.None;
            scoreCount = 0;
            total = 5;
        }



    }

    void Update() {
        score.text = scoreCount.ToString() + "/" + total.ToString();
    }
}
