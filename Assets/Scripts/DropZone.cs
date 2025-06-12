using UnityEngine;

public class DropZone : MonoBehaviour
{
    public HintManager hintManager;
    private void OnTriggerEnter(Collider other) {
       // Debug.Log(other);
        if (other.CompareTag("Capturable+") || other.CompareTag("Capturable-")) {
            //   Debug.Log("Objeto capturado");
            hintManager.ShowHint("Criança capturada", 4f);
            CaptureManager manager = FindFirstObjectByType<CaptureManager>();
            if (manager != null) {
                manager.RegisterCapturedObject(other.gameObject);
            } else {
                Debug.LogError("CaptureManager not found");
            }
        }
    }
}
