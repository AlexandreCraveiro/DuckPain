using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float VelocidadeAndar = 3;
    public float VelocidadeRodar = 30;
    public float VelocidadeSalto = -2;

    float _inputRodar;
    public float _inputAndar;
    public float _movimentoLateral;
    public bool IsGrounded;
    public bool IsJumping;
    public Animator animator;
    Vector3 _velocidade;

    CharacterController controller;

    public AudioClip somPassos;
    AudioSource audioSource;

    // Start
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update
    void Update()
    {
        // Rotação
        _inputRodar = SistemaInput.instance.DeltaRatoX;
        if (_inputRodar != 0)
        {
            transform.Rotate(transform.up * _inputRodar * VelocidadeRodar * Time.deltaTime);
        }

        // Movimento
        _inputAndar = SistemaInput.instance.EixoVertical;
        _movimentoLateral = SistemaInput.instance.EixoHorizontal;

        bool estaAndar = _inputAndar != 0 || _movimentoLateral != 0;

        if (animator != null)
            animator.SetFloat("velocidade", estaAndar ? 1 : 0);

        // Lateral
        if (_movimentoLateral != 0)
        {
            Vector3 vector3 = transform.right * _movimentoLateral * VelocidadeAndar * Time.deltaTime;
            controller.Move(vector3);
        }

        // Correr
        if (SistemaInput.instance.Correr)
        {
            _inputAndar *= 2;
            if (animator != null)
                animator.SetFloat("velocidade", 2);
        }

        // Movimento frontal
        Vector3 movimento = transform.forward * _inputAndar * VelocidadeAndar * Time.deltaTime;
        controller.Move(movimento);

        // Saltar
        if (IsGrounded && SistemaInput.instance.Saltar)
        {
            _velocidade.y = Mathf.Sqrt(VelocidadeSalto * Physics.gravity.y);
            IsJumping = true;
            animator.SetTrigger("jump");
        }
        else
        {
            _velocidade += Physics.gravity * Time.deltaTime;
            IsJumping = false;
        }

        controller.Move(_velocidade * Time.deltaTime);
        IsGrounded = controller.isGrounded;

        
        if (estaAndar && IsGrounded)
{
    if (!audioSource.isPlaying)
    {
        audioSource.clip = somPassos;
        audioSource.Play();
    }

    // Aumenta o pitch se estiver a correr
    if (SistemaInput.instance.Correr)
        audioSource.pitch = 1.5f; // Mais rápido
    else
        audioSource.pitch = 1.0f; // Normal
}
else
{
    audioSource.Stop();
}
    }
}
