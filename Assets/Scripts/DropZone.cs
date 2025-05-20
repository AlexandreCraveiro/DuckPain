using UnityEngine;

public class DropZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Capturable+") || other.CompareTag("Capturable-")) {
            Debug.Log("Objeto capturado");
            CaptureManager manager = FindFirstObjectByType<CaptureManager>();
            if (manager != null) {
                manager.RegisterCapturedObject(other.gameObject);
            } else {
                Debug.LogError("CaptureManager not found");
            }
        }
    }
}
