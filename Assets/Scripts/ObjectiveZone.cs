using UnityEngine;
using TMPro;
using System.Collections;

public class ObjectiveZone : MonoBehaviour
{
    public TextMeshProUGUI resultsText;
    public TextMeshProUGUI score;
    public int total = 3;
    public int scoreCount = 0;


    IEnumerator ShowTemporaryMessage(string msg, float duration, CaptureManager manager)
    {
        resultsText.text = msg;
        yield return new WaitForSeconds(duration);
        resultsText.text = "";
        manager.ResetCounters();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("EnterVehicle")) {
            CaptureManager manager = FindObjectOfType<CaptureManager>();
            if (manager != null) {
                string message = "Foram capturadas " + manager.getCorrectedCount().ToString() + " corretas e " + manager.getIncorrectedCount().ToString() + " incorretas.";
                scoreCount = manager.getCorrectedCount();
                StartCoroutine(ShowTemporaryMessage(message, 2f, manager));
            } else {
                Debug.Log("CaptureManager not found");
            }
        }
    }

    void Update() {
        score.text = scoreCount.ToString() + "/" + total.ToString();
    }
}
