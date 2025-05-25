using UnityEngine;
using UnityEngine.VFX;


public class CarController : MonoBehaviour
{
    public float motorForce = 100f;
    public float brakeForce = 1000f;
    public float maxSteerAngle = 30f;

    private bool arrancou = false;
    private float tempoArranque = 0f;
    public float duracaoEfeitoArranque = 3f;

    private bool podeAtivarArranque = true;

    public VisualEffect EfeitoRodas;

    public VisualEffect FumoCarrinha;

    private float tempoUltimoX = -999f;
    private float tempoCooldownX = 11f;



    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentBrakeForce;
    private bool isBraking;
    private bool jogadorDentro = false;
    public DispararGelados dispararGelado;

    public ControladorSom somControlador;

    private bool somMotorAtivo = false;

    public SomAmbienteComDistancia somParque;



    private void Update()
    {

        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();

        if (!jogadorDentro)
        {
            if (FumoCarrinha != null) FumoCarrinha.SetBool("Emitir", false);
            if (EfeitoRodas != null) EfeitoRodas.SetBool("Emitir", false);
            return;
        }

        if (FumoCarrinha != null)
        {
            bool ativarFumo = Mathf.Abs(verticalInput) > 0.1f;
            FumoCarrinha.SetBool("Emitir", ativarFumo);
        }

        float velocidade = GetComponent<Rigidbody>().linearVelocity.magnitude;

        // Se o carro estiver praticamente parado, permite novo arranque
        if (velocidade < 0.1f)
        {
            podeAtivarArranque = true;
            arrancou = false; // opcional: reforça o reset
        }

        // Ativa o efeito apenas ao arrancar do zero
        if (podeAtivarArranque && velocidade > 0.5f && Mathf.Abs(verticalInput) > 0.1f)
        {
            arrancou = true;
            tempoArranque = duracaoEfeitoArranque;
            podeAtivarArranque = false;

            if (EfeitoRodas != null)
                EfeitoRodas.SetBool("Emitir", true);
        }

        // Desliga o efeito após os 2 segundos
        if (arrancou)
        {
            tempoArranque -= Time.deltaTime;
            if (tempoArranque <= 0f)
            {
                arrancou = false;
                if (EfeitoRodas != null)
                    EfeitoRodas.SetBool("Emitir", false);
            }
        }

        if (Input.GetKeyDown(KeyCode.X) && jogadorDentro && somControlador != null)
        {
            float tempoAtual = Time.time;
            if (tempoAtual - tempoUltimoX >= tempoCooldownX)
            {
                somControlador.PararSomMotor(); // parar som motor
                somControlador.TocarSomPersonalizado(); // tocar som X
                tempoUltimoX = tempoAtual; // regista tempo do último X
            }
        }



    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space);
    }



    public void JogadorEntrouNoCarro()
    {
        jogadorDentro = true;
        if (dispararGelado != null)
            dispararGelado.jogadorDentroDoCarro = true;

        if (somControlador != null)
            somControlador.TocarSomLigarCarro();

        if (somParque != null)
            somParque.jogadorDentroDaCarrinha = true;
    }

    public void JogadorSaiuDoCarro()
    {
        jogadorDentro = false;
        FumoCarrinha.SetBool("Emitir", false);
        //dispararGelado.SaiuDoCarro();

        if (somParque != null)
        somParque.jogadorDentroDaCarrinha = false;
    }



    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;

        currentBrakeForce = isBraking ? brakeForce : 0f;
        ApplyBraking();

        // Ativar ou desativar som do motor conforme movimento
        float velocidade = GetComponent<Rigidbody>().linearVelocity.magnitude;

        if (somControlador != null)
        {
            if (Mathf.Abs(verticalInput) > 0.1f && jogadorDentro && velocidade > 0.2f)
            {
                somControlador.IniciarSomMotor();
            }
            else
            {
                somControlador.PararSomMotor();
            }
        }


    }
    private void ApplyBraking()
    {
        frontLeftWheelCollider.brakeTorque = currentBrakeForce;
        frontRightWheelCollider.brakeTorque = currentBrakeForce;
        rearLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearRightWheelCollider.brakeTorque = currentBrakeForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;
        wheelCollider.GetWorldPose(out position, out rotation);
        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }



}
