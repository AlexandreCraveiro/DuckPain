using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float VelocidadeAndar = 3;
    public float VelocidadeRodar = 30;
    public float VelocidadeSalto = -2;

    float _inputRodar; //unico que n�o � publico porque nao precisa de animacao
    public float _inputAndar; 
    public float _movimentoLateral;
    public bool IsGrounded;
    public bool IsJumping;
     public Animator animator;
    Vector3 _velocidade;

    CharacterController controller;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator != null)
            animator.SetFloat("velocidade", 0);
        //rotacao
        _inputRodar = SistemaInput.instance.DeltaRatoX;
        if (_inputRodar != 0)
        {
            transform.Rotate(transform.up * _inputRodar * VelocidadeRodar * Time.deltaTime);
        }
        //movimento
        _inputAndar = SistemaInput.instance.EixoVertical;
        _movimentoLateral = SistemaInput.instance.EixoHorizontal;
        if (animator != null)
            animator.SetFloat("velocidade", 1);

        //movimento lateral 
        // para bloquear o andar para o lado _inputAndar != 0 && 
        if (_movimentoLateral != 0)
        {
            Vector3 vector3 = transform.right * _movimentoLateral * VelocidadeAndar * Time.deltaTime;
            controller.Move(vector3);
        }
        else
        {
            _movimentoLateral = 0;
        }
        //correr
        if (SistemaInput.instance.Correr)
        {
            _inputAndar *= 2;
            if (animator != null)
            animator.SetFloat("velocidade", 2);
        }
        //movimento
        Vector3 movimento = transform.forward * _inputAndar * VelocidadeAndar * Time.deltaTime;
        controller.Move(movimento);

        //salto
        if (IsGrounded && SistemaInput.instance.Saltar)
        {
            _velocidade.y = Mathf.Sqrt(VelocidadeSalto * Physics.gravity.y);
            IsJumping = true;
        }
        else
        {
            IsJumping = false;
            _velocidade += Physics.gravity * Time.deltaTime;
        }
        controller.Move(_velocidade * Time.deltaTime);
        IsGrounded = controller.isGrounded;

    }
}
