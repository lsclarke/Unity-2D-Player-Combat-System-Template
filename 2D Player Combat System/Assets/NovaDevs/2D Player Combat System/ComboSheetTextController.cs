using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboSheetTextController : MonoBehaviour
{

    public TextMeshProUGUI comboListText;
    public String stringName;

    private void Update()
    {
        comboListText.text = stringName;
    }
}
