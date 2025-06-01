using UnityEngine;
using UnityEngine.AI;

public class NPCCarControllerNav : MonoBehaviour
{
    public enum EstadoNPC { Idle, Patrulha, Perseguir }
    public EstadoNPC estadoAtual = EstadoNPC.Patrulha;

    [Header("Comportamento")]
    public bool inimigo = false;
    public Transform jogador;
    public float distanciaVisao = 15f;
    public float distanciaDesistir = 20f;
    public float velocidade = 5f;
    public float velocidadeAtual;

    [Header("Sirene")]
    public AudioSource sireneAudio;


    [Header("Patrulha")]
    public Transform[] pontos;
    private int proximoPonto = 0;
    public float pontoDeMudanca = 3f;

    [Header("Movimento")]
    public float motorForce = 1000f;
    public float breakForce = 1000f;
    public float maxSteerAngle = 30f;

    [Header("Wheel Colliders")]
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    [Header("Wheel Visuals")]
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;

    [Header("Idle")]
    public float tempoIdle = 1.5f;
    private float tempoAtual = 0f;

    private NavMeshAgent navMeshAgent;
    private PlayerInteraction playerInteraction;
    float TempoColidiuComGelado;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        tempoAtual = tempoIdle;

        if (jogador != null)
        {
            playerInteraction = jogador.GetComponent<PlayerInteraction>();
        }
        velocidadeAtual = velocidade;
        navMeshAgent.speed = velocidadeAtual;
    }

    void FixedUpdate()
    {
        //verifica se � preciso recuperar velocidade
        if (velocidade != velocidadeAtual)
        {
            if (TempoColidiuComGelado + 2 < Time.time)
            {
                velocidadeAtual = velocidade;
                navMeshAgent.speed = velocidadeAtual;
                Debug.Log("Recuperando velocidade ap�s colis�o com gelado.");
            }
        }
        if (inimigo && jogador != null && playerInteraction != null)
        {
            float distanciaJogador = Vector3.Distance(transform.position, jogador.position);

            if (playerInteraction.isInCar)
            {
                if (estadoAtual != EstadoNPC.Perseguir && distanciaJogador <= distanciaVisao)
                {
                    estadoAtual = EstadoNPC.Perseguir;
                }
            }
            else
            {
                if (estadoAtual == EstadoNPC.Perseguir)
                {
                    estadoAtual = EstadoNPC.Idle;
                    tempoAtual = tempoIdle;
                    navMeshAgent.ResetPath();
                }
            }

            if (estadoAtual == EstadoNPC.Perseguir && distanciaJogador > distanciaDesistir)
            {
                estadoAtual = EstadoNPC.Idle;
                tempoAtual = tempoIdle;
                navMeshAgent.ResetPath();
            }
        }

        switch (estadoAtual)
        {
            case EstadoNPC.Idle:
                Estado_Idle();
                break;
            case EstadoNPC.Patrulha:
                Estado_Patrulha();
                break;
            case EstadoNPC.Perseguir:
                Estado_Perseguir();
                break;
        }
        ControlarSirene();

        UpdateWheelVisuals();
    }

    void Estado_Idle()
    {
        PararCarro();

        tempoAtual -= Time.fixedDeltaTime;
        if (tempoAtual <= 0)
        {
            proximoPonto = EncontrarPontoMaisProximo();
            estadoAtual = EstadoNPC.Patrulha;
        }
    }

    void Estado_Patrulha()
    {
        if (pontos.Length == 0)
        {
            estadoAtual = EstadoNPC.Idle;
            return;
        }

        Vector3 destino = pontos[proximoPonto].position;
        float distancia = Vector3.Distance(transform.position, destino);

        ConduzirAte(destino, distancia);

        if (distancia < pontoDeMudanca)
        {
            proximoPonto++;
            if (proximoPonto >= pontos.Length) proximoPonto = 0;
        }
    }

    void Estado_Perseguir()
    {
        if (jogador == null) return;

        Vector3 destino = jogador.position;
        float distancia = Vector3.Distance(transform.position, destino);

        ConduzirAte(destino, distancia);
    }

    void ConduzirAte(Vector3 destino, float distancia)
    {
        Vector3 localTarget = transform.InverseTransformPoint(destino);
        float steer = Mathf.Clamp(localTarget.x / localTarget.magnitude, -1f, 1f);
        float angulo = Mathf.Abs(steer);
        float motor = 1f;

        if (angulo > 0.5f)
            motor *= 0.5f;

        float brake = 0f;

        frontLeftWheel.steerAngle = steer * maxSteerAngle;
        frontRightWheel.steerAngle = steer * maxSteerAngle;

        // Este NPC usa NavMeshAgent para navegar
        navMeshAgent.SetDestination(destino);

        frontLeftWheel.brakeTorque = brake;
        frontRightWheel.brakeTorque = brake;
        rearLeftWheel.brakeTorque = brake;
        rearRightWheel.brakeTorque = brake;
    }

    void PararCarro()
    {
        frontLeftWheel.motorTorque = 0f;
        frontRightWheel.motorTorque = 0f;

        frontLeftWheel.brakeTorque = breakForce;
        frontRightWheel.brakeTorque = breakForce;
        rearLeftWheel.brakeTorque = breakForce;
        rearRightWheel.brakeTorque = breakForce;
    }

    int EncontrarPontoMaisProximo()
    {
        int maisProximo = 0;
        float menorDistancia = Mathf.Infinity;

        for (int i = 0; i < pontos.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, pontos[i].position);
            if (dist < menorDistancia)
            {
                menorDistancia = dist;
                maisProximo = i;
            }
        }

        return maisProximo;
    }

    void UpdateWheelVisuals()
    {
        UpdateWheel(frontLeftWheel, frontLeftWheelTransform);
        UpdateWheel(frontRightWheel, frontRightWheelTransform);
        UpdateWheel(rearLeftWheel, rearLeftWheelTransform);
        UpdateWheel(rearRightWheel, rearRightWheelTransform);
    }

    void UpdateWheel(WheelCollider collider, Transform visual)
    {
        collider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        visual.position = pos;
        visual.rotation = rot;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IceCream"))
        {
            velocidadeAtual -= 0.5f;
            navMeshAgent.speed = velocidadeAtual;
            TempoColidiuComGelado = Time.time;
            Debug.Log("Colidiu com gelado! Velocidade reduzida para: " + velocidadeAtual);
        }
    }
    
    void ControlarSirene()
{
    if (estadoAtual == EstadoNPC.Perseguir)
    {
        if (!sireneAudio.isPlaying)
        {
            sireneAudio.Play();
        }
    }
    else
    {
        if (sireneAudio.isPlaying)
        {
            sireneAudio.Stop();
        }
    }
}

}

