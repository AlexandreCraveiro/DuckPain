using UnityEngine;

public class IceCreamPickup : MonoBehaviour
{
    public Transform holdPoint; // posição onde o gelado vai ficar segurado (assign no inspector)
    public float launchForce = 10f;

    private Rigidbody rb;
    private bool isHeld = false;
    private Transform player;

    private DispararGelados dispararGelados;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            dispararGelados = player.GetComponent<DispararGelados>(); // tenta buscar o script do jogador
            Debug.Log("Gelado perto do jogador, pressiona G para apanhar.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            dispararGelados = null;
            Debug.Log("Gelado longe do jogador.");
        }
    }

    void Update()
    {
        if (player == null || dispararGelados == null) return;

        if (!isHeld && Input.GetKeyDown(KeyCode.G))
        {
            if (dispararGelados.TemGelado())
            {
                isHeld = true;
                dispararGelados.SetTemGelado(false);  // gelado agora está na mão, já não está disponível para disparar
                rb.isKinematic = true;
                transform.position = holdPoint.position;
                transform.parent = holdPoint;
                Debug.Log("Gelado apanhado.");
            }
            else
            {
                Debug.Log("Não tens gelado disponível para apanhar.");
            }
        }
        else if (isHeld && Input.GetKeyDown(KeyCode.F))
        {
            // Lançar gelado
            isHeld = false;
            transform.parent = null;
            rb.isKinematic = false;

            Vector3 launchDir = player.forward + Vector3.up * 0.3f;
            rb.AddForce(launchDir.normalized * launchForce, ForceMode.VelocityChange);

            Debug.Log("Gelado lançado.");
        }

        if (isHeld)
        {
            // Manter o gelado na posição da mão (caso o player se mova)
            transform.position = holdPoint.position;
        }
    }
}
