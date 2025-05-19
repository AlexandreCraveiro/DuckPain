using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Catch : MonoBehaviour
{
    public GameObject circleVisual;
    public float maxRadius = 5f;
    public float growthSpeed = 2f;
    public float shrinkSpeed = 20f;
    public float catchTime = 2f;

    private Vector3 initialScale;
    private bool isHolding = false;
    private float currentScale = 0f;
    private bool isShrinking = false;

    private int nrCorrect = 0;
    private int nrWrong = 0;

    public TextMeshProUGUI capturedText;

    private Dictionary<GameObject, float> objectTimers = new Dictionary<GameObject, float>();

    void Start()
    {
        if (circleVisual != null)
        {
            circleVisual.SetActive(false);
            initialScale = circleVisual.transform.localScale;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isHolding = true;
            currentScale = 0f;
            circleVisual.transform.localScale = Vector3.zero;

            circleVisual.SetActive(true);
        }

        if (Input.GetKey(KeyCode.E) && isHolding)
        {
            currentScale = Mathf.Min(currentScale + growthSpeed * Time.deltaTime, maxRadius);
            circleVisual.transform.localScale = new Vector3(currentScale, 1f, currentScale);
        
            // Verifica quais os objetos dentro da área
            CheckObjectsInRange();
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            isHolding = false;
            isShrinking = true;
        }

        if(isShrinking) {
            currentScale -= shrinkSpeed * Time.deltaTime;
            currentScale = Mathf.Max(currentScale, 0f);

            circleVisual.transform.localScale = new Vector3(currentScale, 1f, currentScale);

            if (currentScale <= 0f)
            {
                isShrinking = false;
                currentScale = 0f; // <- força a ser zero
                circleVisual.transform.localScale = Vector3.zero; // <- garante escala 0
                circleVisual.SetActive(false);
                objectTimers.Clear(); // <- limpa os timers
            }
        }
        


    }

    void CheckObjectsInRange()
    {
        Vector3 center = circleVisual.transform.position;
        float radius = circleVisual.transform.localScale.x / 2f;

        Debug.Log("Raio: " + radius);

        // Assume que os objetos a apanhar têm tag "Capturable"
        Collider[] hits = Physics.OverlapSphere(center, radius);

        foreach (Collider hit in hits)
        {
            GameObject obj = hit.gameObject;
            Debug.Log("Objeto: " + obj.name);
            if (!obj.CompareTag("Capturable+") && !obj.CompareTag("Capturable-")) continue;
            Debug.Log("Objeto dentro do raio: " + obj.name);

            // Zera a velocidade se o objeto tiver o script Inimigo
            NPC kid = obj.GetComponent<NPC>();
            if (kid != null)
            {
                kid.Velocidade = 0;
                Debug.Log("Objeto parado: " + obj.name);
            }

            if (!objectTimers.ContainsKey(obj))
                objectTimers[obj] = 0f;

            objectTimers[obj] += Time.deltaTime;

            // Se estiver tempo suficiente, apanha
            if (objectTimers[obj] >= catchTime)
            {
                Debug.Log("Objeto apanhado: " + obj.name);
                // Exemplo de ação: destruir

                if (obj.CompareTag("Capturable+"))
                { 
                    nrCorrect++;
                    capturedText.text = nrCorrect.ToString();
                }
                else
                {
                    nrWrong++;
                    capturedText.text = nrWrong.ToString();
                }

                Destroy(obj);
                objectTimers.Remove(obj);
            }
        }

        // Limpar objetos que saíram do raio
        List<GameObject> outsideObjects = new List<GameObject>();
        foreach (var pair in objectTimers)
        {
            if (Vector3.Distance(pair.Key.transform.position, center) > radius) {
                NPC kid = pair.Key.GetComponent<NPC>();
                if (kid != null) 
                {
                    kid.Velocidade = 1;
                }

                outsideObjects.Add(pair.Key);
            }


        }

        foreach (var obj in outsideObjects)
            objectTimers.Remove(obj);
    }


}
