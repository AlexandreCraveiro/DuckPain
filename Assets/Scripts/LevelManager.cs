using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public TextMeshProUGUI incorrectedCount;
    public ObjectiveZone objectiveZone;

    void Awake() {
        objectiveZone = FindFirstObjectByType<ObjectiveZone>();
        if (objectiveZone != null) {
            incorrectedCount.text = "Nivel 1 nao foi concluido com sucesso. Foram capturadas " + objectiveZone.wrongKidsCount.ToString() + " criancas erradas.";
        }
    }

    void Update() {
        if (objectiveZone != null) {
            incorrectedCount.text = "Nivel 1 nao foi concluido com sucesso. Foram capturadas " + objectiveZone.wrongKidsCount.ToString() + " criancas erradas.";
        }
    }
}