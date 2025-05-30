using UnityEngine;

public class ApanhaPlayer : MonoBehaviour
{
    public Transform playerHome;
    HintManager hintManager;
    private void Start()
    {
        playerHome = GameObject.Find("PlayerHome").transform;
        hintManager = FindAnyObjectByType<HintManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterController>().enabled = false; 
            other.transform.position = playerHome.position;
            other.transform.rotation = playerHome.rotation;
            hintManager.ShowHint("Um adulto apanhou-te e levou-te de volta para casa.", 5f);
            other.GetComponent<CharacterController>().enabled = true;
        }
    }
}
