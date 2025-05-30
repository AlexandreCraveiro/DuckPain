using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Video;
/// <summary>
/// Controla as animações do NPC
/// </summary>
public class NPCAnima : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agente;
    NPC npc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        agente = GetComponent<NavMeshAgent>();
        npc = GetComponent<NPC>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("velocidade", agente.velocity.magnitude/npc.Velocidade);
        if(npc.Atacou)
        {
            animator.SetTrigger("atacar");
            npc.Atacou = false;
        }
    }
}
