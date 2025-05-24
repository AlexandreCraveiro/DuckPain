using UnityEngine;
using TMPro;
using System.Collections;

public class HintManager : MonoBehaviour
{
    public TextMeshProUGUI hintText;
    private float hintTimer;

    void Start()
    {
        string[] dicas = {
        "Objetivo: Apanhar X crianças e levá-las à instituição.",
        "Usa a carrinha dos gelados para apanhar crianças!",
        "Fora da carrinha, fica a clicar no 'E' para te perseguirem e larga o 'E' para deixarem de te perseguir!",
        "Cuidado, a polícia anda atrás de ti!",
        "Clica no 'F' para lançar gelados e distrair a polícia."
    };
        ShowHintsSequential(dicas, 7f);

    }


    void Update()
    {
        if (hintTimer > 0)
        {
            hintTimer -= Time.deltaTime;
            if (hintTimer <= 0)
            {
                hintText.text = "";
            }
        }
    }

    public void ShowHint(string message, float duration = 3f)
    {
        hintText.text = message;
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
