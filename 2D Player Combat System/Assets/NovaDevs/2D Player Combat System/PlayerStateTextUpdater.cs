using TMPro;
using UnityEngine;

public class PlayerStateTextUpdater : MonoBehaviour
{

    private TextMeshProUGUI modeText;
    public PlayerCombat combat;

    private void Start()
    {
        
        modeText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(combat.isCombatMode)
            modeText.text = $"Player Mode: Combat";
        if (!combat.isCombatMode)
            modeText.text = $"Player Mode: Exploration";

    }
}
