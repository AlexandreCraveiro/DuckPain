using UnityEngine;

public class NPCCarController : MonoBehaviour
{
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

    void FixedUpdate()
    {
        if (pontos.Length == 0) return;
        float distancia = Vector3.Distance(transform.position, pontos[proximoPonto].position);
        Vector3 destino = pontos[proximoPonto].position;
        Vector3 localTarget = transform.InverseTransformPoint(destino);
        float steer = Mathf.Clamp(localTarget.x / localTarget.magnitude, -1f, 1f);
        float motor = distancia > stoppingDistance * 3 ? 1f : 0.1f;
        float brake = 1 - motor;
        // Debug.Log($"{distancia} Steer: {steer}, Motor: {motor}");
        frontLeftWheel.steerAngle = steer * maxSteerAngle;
        frontRightWheel.steerAngle = steer * maxSteerAngle;

        frontLeftWheel.motorTorque = motor * motorForce;
        frontRightWheel.motorTorque = motor * motorForce;
        frontLeftWheel.brakeTorque = brake * breakForce;
        frontLeftWheel.brakeTorque = brake * breakForce;
        frontLeftWheel.brakeTorque = brake * breakForce;
        frontLeftWheel.brakeTorque = brake * breakForce;

        UpdateWheelVisuals();

        if (distancia< stoppingDistance)
        {
            proximoPonto++;
            if (proximoPonto >= pontos.Length) proximoPonto = 0;
            //transform.LookAt(pontos[proximoPonto]);

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
