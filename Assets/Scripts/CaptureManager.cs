using UnityEngine;
using TMPro;
using System.Collections;

public class CaptureManager : MonoBehaviour
{
    public int correctedCount = 0;
    public int incorrectedCount = 0;

    public TextMeshProUGUI fullVanText;

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
            StartCoroutine(ShowTemporaryMessage("A carrinha está cheia!", 2f));
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

        Destroy(capturedObject);
    }
}
