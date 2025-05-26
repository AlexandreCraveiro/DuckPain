using UnityEngine;
using TMPro;
using System.Collections;

public class CaptureManager : MonoBehaviour
{
    public int correctedCount = 0;
    public int incorrectedCount = 0;

    public TextMeshProUGUI fullVanText;
        public HintManager hintManager; // referenciar no Inspector


    IEnumerator ShowTemporaryMessage(string msg, float duration)
    {
        fullVanText.text = msg;
        yield return new WaitForSeconds(duration);
        fullVanText.text = "";
    }

    public void RegisterCapturedObject(GameObject capturedObject)
    {

        if (correctedCount + incorrectedCount >= 2) {
            Debug.Log("A carrinha está cheia");
            //StartCoroutine(ShowTemporaryMessage("A carrinha está cheia!", 2f));
            hintManager.ShowHint("A carrinha está cheia! Leva as crianças à instituição", 5f);

            return;
        }

        if (capturedObject.CompareTag("Capturable+"))
        {
            correctedCount++;
        }
        else if (capturedObject.CompareTag("Capturable-"))
        {
            incorrectedCount++;
        }

        Debug.Log("Objeto capturado: " + capturedObject.name);
        Destroy(capturedObject);
    }

    public void ResetCounters()
    {
        correctedCount = 0;
        incorrectedCount = 0;
    }

    public int getCorrectedCount() {
        return correctedCount;
    }

    public int getIncorrectedCount() {
        return incorrectedCount;
    }
}
