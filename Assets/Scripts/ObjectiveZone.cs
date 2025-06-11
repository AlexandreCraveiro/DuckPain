using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class ObjectiveZone : MonoBehaviour
{
    public TextMeshProUGUI resultsText;
    public TextMeshProUGUI score;
    public int total = 3;
    public int wrongKidsCount = 0;
    public int scoreCount = 0;
    public GameObject barreira;
    public GameObject painelsucesso;
    public GameObject painellose;
    public GameObject kids2;
    IEnumerator ShowTemporaryMessage(string msg, float duration, CaptureManager manager)
    {
        resultsText.text = msg;
        yield return new WaitForSeconds(duration);
        resultsText.text = "";
        manager.ResetCounters();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("EnterVehicle")) {
            CaptureManager manager = FindFirstObjectByType<CaptureManager>();
            if (manager != null) {
                string message = "Foram capturadas " + manager.getCorrectedCount().ToString() + " corretas e " + manager.getIncorrectedCount().ToString() + " incorretas.";
                scoreCount = scoreCount + manager.getCorrectedCount();
                wrongKidsCount = wrongKidsCount + manager.getIncorrectedCount();
                manager.ResetCounters();
                // StartCoroutine(ShowTemporaryMessage(message, 2f, manager));
                if (scoreCount == total && wrongKidsCount == 0) {
                    PlayerPrefs.SetInt("level", 1);
                    PlayerPrefs.Save();
                    Time.timeScale = 0f; // Pausa o jogo
                    painelsucesso.SetActive(true);
                    Destroy(barreira);
                    Cursor.lockState = CursorLockMode.None;
                    scoreCount = 0;
                    total = 5;
                    kids2.SetActive(true);
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

    void Update() {
        score.text = scoreCount.ToString() + "/" + total.ToString();
    }
}
