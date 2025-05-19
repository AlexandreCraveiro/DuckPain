using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{

    public TMP_Text NrCapturados; 

    void Start()
    {
        UpdateHUD();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHUD();
    }

    void UpdateHUD()
    {
        NrCapturados.text = Catch.NrCorrect.ToString();
    }
}
