using UnityEngine;
using UnityEngine.InputSystem;

public class SistemaInput : MonoBehaviour
{
    public InputSystem_Actions inputActions;
    public static SistemaInput instance; //Singleton, só existe uma vez no projeto
    //guardar os dados de input
    public float EixoHorizontal;  //esquerda e direita
    public float EixoVertical;  //cima e baixo
    public float DeltaRatoX; //movimento do rato na horizontal
    public float DeltaRatoY; //movimento do rato na vertical
    public bool Correr;
    public bool Saltar;
    public bool Empurrar;
    public bool TeclaEsc;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        inputActions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    private void OnDestroy()
    {
        instance = null;
    }
    //ler o movimento
    public void LerMovimento()
    {
        Vector2 movimento = inputActions.Player.Move.ReadValue<Vector2>();
        EixoHorizontal = movimento.x;
        EixoVertical = movimento.y;
    }
    //ler o rato
    public void LerRato()
    {
        Vector2 rato = inputActions.Player.Look.ReadValue<Vector2>();
        DeltaRatoX = rato.x;
        DeltaRatoY = rato.y;
    }
    //ler o correr
    public void LerCorrer()
    {
        if (inputActions.Player.Sprint.ReadValue<float>() > 0)
            Correr = true;
        else
            Correr = false;
    }
    //ler o saltar
    public void LerSaltar()
    {
        if (inputActions.Player.Jump.triggered)
            Saltar = true;
        else
            Saltar = false;
    }
    //ler o empurrar
    public void LerEmpurrar()
    {
        if (inputActions.Player.Push.triggered)
            Empurrar = true;
        else
            Empurrar = false;
    }
    //ler a tecla ESC
    public void LerTeclaEsc()
    {
        if (inputActions.Player.Escape.triggered)
            TeclaEsc = true;
        else
            TeclaEsc = false;
    }
    // Update is called once per frame
    void Update()
    {
        LerMovimento();
        LerRato();
        LerCorrer();
        LerSaltar();
        LerEmpurrar();
        LerTeclaEsc();
    }
}
