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
    private bool musicaATocar = false;

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
    public SomAmbienteComDistancia somParque;

    private bool estavaATravar = false;
    private bool efeitoTravagemAtivo = false;

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

        if (velocidade < 0.1f)
        {
            podeAtivarArranque = true;
            arrancou = false;
        }

        if (podeAtivarArranque && velocidade > 0.5f && Mathf.Abs(verticalInput) > 0.1f)
        {
            arrancou = true;
            tempoArranque = duracaoEfeitoArranque;
            podeAtivarArranque = false;

            if (EfeitoRodas != null)
                EfeitoRodas.SetBool("Emitir", true);
        }

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

        // ⬇️ Alternar música personalizada com a tecla X
        if (Input.GetKeyDown(KeyCode.X) && jogadorDentro && somControlador != null)
        {
            if (somControlador.ATocarMusica())
            {
                somControlador.PararSomPersonalizado();
            }
            else
            {
                somControlador.IniciarSomPersonalizado();
            }
            //float tempoAtual = Time.time;
            //if (tempoAtual - tempoUltimoX >= tempoCooldownX)
            //{
            //    if (!musicaATocar)
            //    {
            //        somControlador.PararSomMotor();
            //        somControlador.IniciarSomPersonalizado();
            //        musicaATocar = true;
            //    }
            //    else
            //    {
            //        somControlador.PararSomPersonalizado();
            //        somControlador.IniciarSomMotor(); // opcional
            //        musicaATocar = false;
            //    }

            //    tempoUltimoX = tempoAtual;
            //}
        }

        if (jogadorDentro && isBraking && !estavaATravar && somControlador != null)
        {
            float volumeTravagem = Mathf.Clamp(velocidade / 20f * 0.6f, 0.1f, 0.6f);
            somControlador.TocarSomTravagem(volumeTravagem);
        }

        estavaATravar = isBraking;

        if (EfeitoRodas != null)
            EfeitoRodas.SetBool("Emitir", isBraking);
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space);

        if (isBraking && !efeitoTravagemAtivo)
        {
            if (EfeitoRodas != null)
                EfeitoRodas.SetBool("Emitir", true);

            efeitoTravagemAtivo = true;
        }
        else if (!isBraking && efeitoTravagemAtivo)
        {
            if (EfeitoRodas != null)
                EfeitoRodas.SetBool("Emitir", false);

            efeitoTravagemAtivo = false;
        }
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

        if (dispararGelado != null)
        {
            dispararGelado.SaiuDoCarro();
            dispararGelado.RecolherGelado();
        }

        if (somParque != null)
            somParque.jogadorDentroDaCarrinha = false;
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;

        currentBrakeForce = isBraking ? brakeForce : 0f;
        ApplyBraking();

        float velocidade = GetComponent<Rigidbody>().linearVelocity.magnitude;

        if (somControlador != null && !musicaATocar)
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
