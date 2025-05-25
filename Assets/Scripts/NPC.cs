using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Gere o comportamento de um NPC
/// </summary>
public class NPC : MonoBehaviour
{
    public bool Inimigo = true;
    public enum NPCEstados { Idle = 0, Patrulha = 1, Atacar = 2 }
    public NPCEstados Estado = NPCEstados.Idle; //estado inicial
    public Transform[] Pontos; //array de pontos de patrulha
    public int ProximoPonto = 0;
    public int Velocidade = 3;
    public float DistanciaMinima = 1;
    public float DistanciaAtacar = 1;
    public float DistanciaVisao = 10;
    public float IntervaloAtacar = 3;
    public float IntervaloAtual = 0;
    NavMeshAgent Agente;
    GameObject Jogador;
    public Transform Olhos;
    public float AnguloVisao = 90;
    public bool Atacou = false;
    public float TempoEspera = 5; //tempo que fica parado quando deixa de ver o player
    public float TempoAEspera = 0;
    public Animator animator; //referência ao Animator do NPC
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Jogador = GameObject.FindGameObjectWithTag("Player");
        if (Jogador == null)
        {
            Debug.LogError("Jogador não encontrado");
        }
        Agente = GetComponent<NavMeshAgent>();
        TempoAEspera = TempoEspera;
        animator = GetComponent<Animator>();
    }
    void Estado_Idle()
    {
        if (Agente == null)
        {
            return;
        }
        Agente.isStopped = true;
        Agente.speed = 0;
        Agente.velocity = Vector3.zero;
        Estado = NPCEstados.Idle;
        if(animator != null)
            animator.SetFloat("velocidade",0); // Define a velocidade do NPC no Animator para 0
    }
    void Estado_Patrulha()
    {
        if (Agente == null)
        {
            return;
        }
        if (Pontos.Length == 0)
        {
            Estado = NPCEstados.Idle;
            return;
        }
        if (Agente.isOnNavMesh)
            Agente.isStopped = false;
        Agente.speed = Velocidade;
        if(Vector3.Distance(transform.position, Pontos[ProximoPonto].position) < DistanciaMinima)
        {
            ProximoPonto++; //avança para o próximo ponto
            if (ProximoPonto >= Pontos.Length) //se chegou ao ultimo ponto volta ao 0
            {
                ProximoPonto = 0;
            }
        }
        Agente.SetDestination(Pontos[ProximoPonto].position);
        if (animator != null)
            animator.SetFloat("velocidade", 1);
    }
    void Estado_Atacar()
    {
        Agente.speed = Velocidade * 1.5f;
        Vector3 OlharPara = new Vector3(Jogador.transform.position.x, transform.position.y, Jogador.transform.position.z);
        transform.LookAt(OlharPara);
        //pode atacar
        if (Vector3.Distance(transform.position, Jogador.transform.position) < DistanciaAtacar)
        {
            Agente.isStopped = true;
            Agente.velocity = Vector3.zero;
            Atacou = true;
        }
        else
        {
            Agente.isStopped = false;
            Agente.SetDestination(Jogador.transform.position);
        }
        if (animator != null)
            animator.SetFloat("velocidade", 2);
    }
    //Devolve true se vê o player
    bool VePlayer()
    {
        if (Vector3.Distance(transform.position, Jogador.transform.position) < DistanciaVisao)
        {
            return true;
        }
        //verificar se vê o player
        if (Utils.CanYouSeeThis(Olhos, Jogador.transform, "Player", AnguloVisao, DistanciaVisao))
        {
            return true;
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Estado == NPCEstados.Idle)
        {
            Estado_Idle();
            if(Inimigo && VePlayer())
            {
                Estado = NPCEstados.Atacar;
            }
        }
        else if (Estado == NPCEstados.Patrulha)
        {
            Estado_Patrulha();
            if (Inimigo && VePlayer())
            {
                Estado = NPCEstados.Atacar;
            }
        }
        else if (Estado == NPCEstados.Atacar)
        {
            if (Inimigo == false)
            {
                Estado = NPCEstados.Patrulha;
                return;
            }
            if (VePlayer())
            {
                Estado_Atacar();
                TempoAEspera = TempoEspera;
            }
            else
            {
                TempoAEspera -= Time.deltaTime;
                if (TempoAEspera <= 0)
                {
                    Estado = NPCEstados.Patrulha;
                    TempoAEspera = TempoEspera;
                }
            }
        }
    }
}
