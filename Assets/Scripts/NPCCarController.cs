using UnityEngine;

public class NPCCarController : MonoBehaviour
{
    public bool inimigo = false;
    public Transform jogador;
    public float distanciaParaPerseguir = 15f;

    public Transform[] pontos;
    public float motorForce = 1000f;
    public float breakForce = 1000f;
    public float maxSteerAngle = 30f;
    public float stoppingDistance = 3f;

    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;

    private int proximoPonto = 0;
    private bool aPerseguir = false;

    void FixedUpdate()
    {
        if (inimigo && jogador != null)
        {
            float distanciaJogador = Vector3.Distance(transform.position, jogador.position);
            aPerseguir = distanciaJogador < distanciaParaPerseguir;
        }

        Vector3 destino = aPerseguir ? jogador.position : pontos[proximoPonto].position;

        float distancia = Vector3.Distance(transform.position, destino);
        Vector3 localTarget = transform.InverseTransformPoint(destino);
        float steer = Mathf.Clamp(localTarget.x / localTarget.magnitude, -1f, 1f);
        float motor = distancia > stoppingDistance * 3 ? 1f : 0.1f;
        float brake = 1 - motor;

        frontLeftWheel.steerAngle = steer * maxSteerAngle;
        frontRightWheel.steerAngle = steer * maxSteerAngle;

        frontLeftWheel.motorTorque = motor * motorForce;
        frontRightWheel.motorTorque = motor * motorForce;

        frontLeftWheel.brakeTorque = brake * breakForce;
        frontRightWheel.brakeTorque = brake * breakForce;
        rearLeftWheel.brakeTorque = brake * breakForce;
        rearRightWheel.brakeTorque = brake * breakForce;

        UpdateWheelVisuals();

        if (!aPerseguir && distancia < stoppingDistance)
        {
            proximoPonto++;
            if (proximoPonto >= pontos.Length) proximoPonto = 0;
        }
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

