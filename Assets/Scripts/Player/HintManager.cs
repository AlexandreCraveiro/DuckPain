using UnityEngine;
using TMPro;
using System.Collections;

public class HintManager : MonoBehaviour
{
    public TextMeshProUGUI hintText;
    private float hintTimer;
    public UnityEngine.UI.Image hintBackground;
    ObjectiveZone objectiveZone;

    void Start()
    {
        objectiveZone = FindAnyObjectByType<ObjectiveZone>();

        string[] dicas = {
        "Objetivo: Apanhar 3 crianças e levá-las à instituição.",
        "Usa a carrinha dos gelados para apanhar crianças!",
        "Fora da carrinha, fica a clicar no 'E' para te perseguirem e larga o 'E' para deixarem de te perseguir!",
        "Cuidado, a polícia anda atrás de ti!",
        "Clica no 'F' para lançar gelados e distrair a polícia.",
        "Podes voltar a apanhar os gelados."
        };
        if (objectiveZone.nivel == 0)
            ShowHintsSequential(dicas, 4f);
        else
            HideHint();
    }


    void Update()
    {
        if (hintTimer > 0)
        {
            hintTimer -= Time.deltaTime;
            if (hintTimer <= 0)
            {
                HideHint();
            }
        }
    }

    public void HideHint()
    {
        hintText.text = "";
        hintBackground.enabled = false; // <-- Esconde o fundo
    }

    public void ShowHint(string message, float duration = 3f)
    {
        hintText.text = message;
        hintBackground.enabled = true; // <-- Ativa o fundo
        hintTimer = duration;
    }


    public void ShowHintsSequential(string[] messages, float durationPerHint = 3f)
    {
        StartCoroutine(ShowHintsRoutine(messages, durationPerHint));
    }

    private IEnumerator ShowHintsRoutine(string[] messages, float duration)
    {
        foreach (string msg in messages)
        {
            ShowHint(msg, duration);
            yield return new WaitForSeconds(duration);
        }
        // Opcional: limpa o texto no fim
        ShowHint("");
    }
}
