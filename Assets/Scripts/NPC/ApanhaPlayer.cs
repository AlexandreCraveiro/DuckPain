using UnityEngine;

public class ApanhaPlayer : MonoBehaviour
{
    public Transform playerHome;
    HintManager hintManager;
    public bool Policia = false; // Define se é a polícia que apanha o jogador
    public GameObject GameOver;
    ObjectiveZone objectiveZone;
    private void Start()
    {
        playerHome = GameObject.Find("PlayerHome").transform;
        hintManager = FindAnyObjectByType<HintManager>();
        objectiveZone = FindAnyObjectByType<ObjectiveZone>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!Policia)
            {
                other.GetComponent<CharacterController>().enabled = false;
                other.transform.position = playerHome.position;
                other.transform.rotation = playerHome.rotation;
                hintManager.ShowHint("Um adulto apanhou-te e levou-te de volta para casa.", 5f);
                other.GetComponent<CharacterController>().enabled = true;
            }
            else
            {
                Time.timeScale = 0f; // Pausa o jogo
                GameOver.SetActive(true);
                Cursor.lockState = CursorLockMode.None; // Mostra o cursor do rato
                objectiveZone.apanhado = true; // Marca que o jogador foi apanhado
            }
        }
    }
}
