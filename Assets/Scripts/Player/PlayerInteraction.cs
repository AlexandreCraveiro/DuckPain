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

        // Ativa o script de condução
        carController.enabled = true;

        // Desativa o controlo da personagem
        playerModel.GetComponent<PlayerMove>().enabled = false;

        //muda o target da camera para o carro
        cinemachineThirdPersonAim.GetComponent<CinemachineCamera>().Follow = CarCameraTarget;

        isInCar = true;
    }

    private void ExitCar()
    {
        Debug.Log("Saiu do carro");
        playerModel.transform.position = posicaosaida.position;
        playerModel.SetActive(true);
        isInCar = false;
        carController.enabled = false;
        playerModel.GetComponent<PlayerMove>().enabled = true;
        cinemachineThirdPersonAim.GetComponent<CinemachineCamera>().Follow = playerCameraTarget;
    }
}

