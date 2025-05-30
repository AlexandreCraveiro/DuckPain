using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Gere o comportamento de um NPC
/// </summary>
public class NPC : MonoBehaviour
{
    public bool Inimigo = true;
    public enum NPCEstados { Idle = 0, Patrulha = 1, Atacar = 2, Perseguir, ApanhaGelado }
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
    public Animator animator; //refer�ncia ao Animator do NPC
    public ControladorSom controladorSom;
    public string tag_criancas =  "Capturable-" ;
    public CarController carro; //referência ao CarController, se o NPC for um carro
    public float DistanciaOuvir = 20f;
    public float DistanciaParaGeladosGratis = 5f; //distância para procurar gelados grátis
    GameObject geladoApanhar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Jogador = GameObject.FindGameObjectWithTag("Player");
        if (Jogador == null)
        {
            Debug.LogError("Jogador n�o encontrado");
        }
        Agente = GetComponent<NavMeshAgent>();
        TempoAEspera = TempoEspera;
        animator = GetComponent<Animator>();
        controladorSom = FindAnyObjectByType<ControladorSom>();
        carro = FindAnyObjectByType<CarController>();
    }
    // Devoive um GameObject se encontrar um gelado grátis dentro da distância especificada, ou null se não encontrar
    GameObject ProcuraGeladosGratis()
    {
        Collider[] geladosGratis = Physics.OverlapSphere(transform.position, DistanciaParaGeladosGratis);
        foreach (Collider col in geladosGratis)
        {
            if (col.CompareTag("FreeIceCream"))
            {
                return col.gameObject;
            }
        }
        return null;
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
        Agente.stoppingDistance = 0;
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
            ProximoPonto++; //avanca para o proximo ponto
            if (ProximoPonto >= Pontos.Length) //se chegou ao ultimo ponto volta ao 0
            {
                ProximoPonto = 0;
            }
        }
        Agente.SetDestination(Pontos[ProximoPonto].position);
        if (animator != null)
            animator.SetFloat("velocidade", 1);
    }
    void Estado_Perseguir()
    {
        Agente.SetDestination(carro.transform.position);
        Agente.stoppingDistance = 3;
        animator.SetFloat("velocidade", Agente.velocity.magnitude);
    }
    void Estado_Atacar()
    {
        Agente.stoppingDistance = 0;
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
    void Estado_Apanhar_Gelado()
    {
        if (geladoApanhar==null)
        {
            Estado = NPCEstados.Patrulha;
            return;
        }
        if (Vector3.Distance(transform.position,geladoApanhar.transform.position)<1)
        {
            Destroy(geladoApanhar);
            Estado = NPCEstados.Patrulha;
            return;
        }
        Agente.SetDestination(geladoApanhar.transform.position);
    }
    //Devolve true se v� o player
    bool VePlayer()
    {
        if (Vector3.Distance(transform.position, Jogador.transform.position) < DistanciaVisao)
        {
            return true;
        }
        //verificar se v� o player
        if (Utils.CanYouSeeThis(Olhos, Jogador.transform, "Player", AnguloVisao, DistanciaVisao))
        {
            return true;
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        //se é criança (tag começa por Capturable) procura gelados grátis
        if (this.tag.StartsWith("Capturable"))
        {
            GameObject geladoGratis = ProcuraGeladosGratis();
            if (geladoGratis != null)
            {
                Estado = NPCEstados.ApanhaGelado;
                geladoApanhar = geladoGratis;
            }
        }
        //se é criança, se está à distancia de ouvir e está a tocar a musica muda o estado para perseguir
        if (Estado != NPCEstados.Perseguir &&
            Vector3.Distance(this.transform.position, carro.transform.position) < DistanciaOuvir &&
            this.CompareTag(tag_criancas) &&
            controladorSom.ATocarMusica())
        {
            Estado = NPCEstados.Perseguir;
        }
        //deixa de perseguir se a música parou ou a distância é muito grande
        if (Estado == NPCEstados.Perseguir &&
           this.CompareTag(tag_criancas) &&
           (Vector3.Distance(this.transform.position, carro.transform.position) > DistanciaOuvir || controladorSom.ATocarMusica() == false))
        {

            Estado = NPCEstados.Patrulha;
        }
        if (Estado == NPCEstados.Idle)
        {
            Estado_Idle();
            if (Inimigo && VePlayer())
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
        else if (Estado == NPCEstados.Perseguir)
        {
            Estado_Perseguir();
        }
        else if(Estado == NPCEstados.ApanhaGelado)
        {
            Estado_Apanhar_Gelado();
        }
    }
}
