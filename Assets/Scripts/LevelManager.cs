using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public TextMeshProUGUI incorrectedCount;

    void Awake() {
        ObjectiveZone objectiveZone = FindFirstObjectByType<ObjectiveZone>();
        incorrectedCount.text = "Nivel 1 nao foi concluido com sucesso. Foram capturadas " + objectiveZone.wrongKidsCount.ToString() + " criancas erradas.";
    }
}