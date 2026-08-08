using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpeedMeterController : MonoBehaviour
{
    public TextMeshProUGUI speedText;
    public Slider speedMeter;

    public PlayerMovement playerMovement;

    private void Update()
    {
        //Text
        speedText.text = $"Speed: {playerMovement.movementSpeed}";

        //Slider
        speedMeter.maxValue = playerMovement.maxMovementSpeed;
        speedMeter.value = playerMovement.movementSpeed;
    }
}
