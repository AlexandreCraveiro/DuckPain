using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject playerModel; // o modelo visível da personagem
    public GameObject car;         // referência ao carro
    public Transform seat;         // posição onde o jogador "entra"
    public CarController carController; // script de condução
    private bool canEnter = false;
    public CinemachineThirdPersonAim cinemachineThirdPersonAim;
    public Transform CarCameraTarget;
    public bool isInCar = false;
    public Transform posicaosaida;
    public Transform playerCameraTarget;
    public HintManager hintManager; // referenciar no Inspector

    private Rigidbody carroRigidbody;

    void Start()
    {
        carroRigidbody = car.GetComponent<Rigidbody>();

        // Travar o carro ao iniciar (caso o jogador comece fora dele)
        if (!isInCar && carroRigidbody != null)
        {
            carroRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canEnter && !isInCar)
            {
                EnterCar();
            }
            else if (isInCar)
            {
                ExitCar();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canEnter = true;
            if (!isInCar)
            {
                hintManager.ShowHint("Clica no 'E' para entrar", 0);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canEnter = false;
            if (!isInCar)
            {
                hintManager.HideHint(); // Esconde a dica se o jogador sair do trigger
            }
        }
    }

    void EnterCar()
    {
        // Esconde o modelo da personagem
        playerModel.SetActive(false);

        // Move o objeto Player para dentro do carro
        transform.position = seat.position;
        transform.rotation = seat.rotation;

        // Liberta o carro para se mover
        if (carroRigidbody != null)
        {
            carroRigidbody.constraints = RigidbodyConstraints.None;
        }

        // Ativa o script de condução
        carController.enabled = true;
        carController.JogadorEntrouNoCarro();

        // Desativa o controlo da personagem
        playerModel.GetComponent<PlayerMove>().enabled = false;

        // Muda o target da câmara para o carro
        cinemachineThirdPersonAim.GetComponent<CinemachineCamera>().Follow = CarCameraTarget;

        isInCar = true;
        carController.FumoCarrinha.SetBool("Emitir", true);
        string[] dicas = {
        "Clica no 'E' para sair",
        "Clica no 'X' para tocar a música.",
        "A música atrai as crianças que não queres apanhar.",
        "Quando a carrinha estiver cheia, leva as crianças à instituição"
    };
        hintManager.ShowHintsSequential(dicas, 5f);

        // Corrige bug de reentrada fora do trigger
        canEnter = false;
    }

    private void ExitCar()
    {
        Debug.Log("Saiu do carro");

        // Posiciona o jogador fora do carro
        playerModel.transform.position = posicaosaida.position;
        playerModel.SetActive(true);
        isInCar = false;

        carController.JogadorSaiuDoCarro();
        carController.enabled = false;

        playerModel.GetComponent<PlayerMove>().enabled = true;

        // Volta a mudar a câmara para o jogador
        cinemachineThirdPersonAim.GetComponent<CinemachineCamera>().Follow = playerCameraTarget;

        // Mostra nova dica
        hintManager.ShowHint("Clica no 'E' para entrar", 2f);

        // Imobiliza o carro
        if (carroRigidbody != null)
        {
            carroRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}

