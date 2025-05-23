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

    [Header("Patrulha")]
    public Transform[] pontos;
    private int proximoPonto = 0;
    public float pontoDeMudanca = 3f; // distância para mudar de ponto

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

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        tempoAtual = tempoIdle;
    }

    void FixedUpdate()
    {
        if (inimigo && jogador != null)
        {
            float distanciaJogador = Vector3.Distance(transform.position, jogador.position);

            if (estadoAtual != EstadoNPC.Perseguir && distanciaJogador <= distanciaVisao)
            {
                estadoAtual = EstadoNPC.Perseguir;
            }
            else if (estadoAtual == EstadoNPC.Perseguir && distanciaJogador > distanciaDesistir)
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

        float brake = 0f; // não travamos em patrulha

        frontLeftWheel.steerAngle = steer * maxSteerAngle;
        frontRightWheel.steerAngle = steer * maxSteerAngle;

        //frontLeftWheel.motorTorque = motor * motorForce;
        //frontRightWheel.motorTorque = motor * motorForce;
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
}
