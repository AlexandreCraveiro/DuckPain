using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject playerModel; // o modelo vis�vel da personagem
    public GameObject car;         // refer�ncia ao carro
    public Transform seat;         // posi��o onde o jogador "entra"
    public CarController carController; // script de condu��o
    private bool canEnter = false;
    public CinemachineThirdPersonAim cinemachineThirdPersonAim;
    public Transform CarCameraTarget;
    public bool isInCar = false;
    public Transform posicaosaida;
    public Transform playerCameraTarget;

    


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canEnter && isInCar == false)
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
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canEnter = false;
        }
    }

    void EnterCar()
    {


        // Esconde o modelo da personagem
        playerModel.SetActive(false);

        // Move o objeto Player para dentro do carro
        transform.position = seat.position;
        transform.rotation = seat.rotation;

        // Ativa o script de condu��o
        carController.enabled = true;
        carController.JogadorEntrouNoCarro();

        // Desativa o controlo da personagem
        playerModel.GetComponent<PlayerMove>().enabled = false;

        //muda o target da camera para o carro
        cinemachineThirdPersonAim.GetComponent<CinemachineCamera>().Follow = CarCameraTarget;

        isInCar = true;
        
        carController.FumoCarrinha.SetBool("Emitir", true);
    }

    private void ExitCar()
    {

        

        Debug.Log("Saiu do carro");
        playerModel.transform.position = posicaosaida.position;
        playerModel.SetActive(true);
        isInCar = false;

        carController.JogadorSaiuDoCarro();

        carController.enabled = false;
        playerModel.GetComponent<PlayerMove>().enabled = true;
        cinemachineThirdPersonAim.GetComponent<CinemachineCamera>().Follow = playerCameraTarget;

    


    }
}

